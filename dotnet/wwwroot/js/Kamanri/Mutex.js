import { Sleep } from './Utils.js'

function Mutex() {
    
    this.mutex = false;
    this.Wait = async() => {
        while (this.mutex == true)
            await Sleep(500);
        this.mutex = true;
    };
    this.Signal = () => {
        this.mutex = false;
    };
    return this;
}

//生产者消费者模型
function Critical(value,min,max)
{
    this.value = value;//资源量
    this.mutex = new Mutex();//资源互斥锁
    this.putMutex = new Mutex();//生产互斥锁
    this.getMutex = new Mutex();//消费互斥锁
    this.min = min;//资源最小值
    this.max = max;//资源最大值
    this.producerSwitch = [];//生产者开关
    this.consumerSwitch = [];//消费者开关
    this.producerIDUsed = [];//生产者分配ID使用情况
    this.consumerIDUsed = [];//消费者分配ID使用情况

    this.WaitNotFull = async(putValue) => {
        while(this.value + putValue > this.max)
            await Sleep(500);
    }
    
    this.WaitNotEmpty = async(getValue) => {
        while(this.value - getValue < this.min)
            await Sleep(500);
    }

    this.PutData = async(putValue,Handler) => {
        
        await this.putMutex.Wait();
        await this.WaitNotFull(putValue);
        await this.mutex.Wait();
        this.value += putValue;
        this.mutex.Signal();
        Handler(this.value);
        this.putMutex.Signal();
    }
    this.GetData = async(getValue,Handler) => {
        
        await this.getMutex.Wait();
        await this.WaitNotEmpty(getValue);
        await this.mutex.Wait();
        this.value -= getValue;
        this.mutex.Signal();
        Handler(this.value);
        this.getMutex.Signal(); 
    }

    this.Producer = async(putValue,Handler,time,id) => {
        if(this.producerIDUsed[id] === true) return;
        this.producerIDUsed[id] = true;
        while(this.producerSwitch[id] === true)
        {
            await this.PutData(putValue,Handler);  
            await Sleep(time);
        }
        
    }
    this.Consumer = async(getValue,Handler,time,id) => {
        if(this.consumerIDUsed[id] === true) return;
        this.consumerIDUsed[id] = true;
        while(this.consumerSwitch[id] === true)
        {
            await this.GetData(getValue,Handler);
            await Sleep(time);
        }
        
    }
    this.GetProducerID = () => {
        //找到第一个值不为true的id
        let id = 0;
        while(this.producerSwitch[id] === true) id++;
        this.producerSwitch[id] = true;
        return id;
    }
    this.GetConsumerID = () => {
        let id = 0;
        while(this.consumerSwitch[id] === true) id++;
        this.consumerSwitch[id] = true;
        return id;
    }
    this.ProducerStop = (id) =>{
        this.producerIDUsed[id] = false;
        this.producerSwitch[id] = false;
    }
    this.ConsumerStop = (id) => {
        this.consumerIDUsed[id] = false;
        this.consumerSwitch[id] = false;
    }
}

export {
    Mutex, Critical
}