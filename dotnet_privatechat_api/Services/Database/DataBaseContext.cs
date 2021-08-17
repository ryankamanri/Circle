using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Dynamic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using dotnet.Model.Relation;
using dotnet.Services.Self;
using dotnet.Model;


namespace dotnet.Services.Database
{

    /// <summary>
    /// 数据库上下文类,用于暂存数据库的部分数据  
    /// 所有对数据库关系的操作基于这个类进行
    /// </summary>
    public class DataBaseContext
    {

        #region Attributes
        private SQL _sql;

        public User user;

        public Post post;

        public Tag tag;

        public ID_ID id_id;

        private KeyComparer keyComparer;

        private ValueComparer valueComparer;


        #endregion
        




        
        public DataBaseContext(SQL sql)
        {
            _sql = sql;

            user = new User();
            post = new Post();
            tag = new Tag();
            id_id = new ID_ID();
            keyComparer = new KeyComparer();
            valueComparer = new ValueComparer();
            
            
        }

        



        /// <summary>
        /// 将模型实体保存到数据库
        /// </summary>
        /// <param name="te"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task Insert<TEntity>(TEntity te)
        {
            dynamic Te = te;
            string SQLStatement = $"insert into {Te.TableName} ({Te.ColumnsWithoutID}) values ({Te.InsertString()})";
            await _sql.Execute(SQLStatement);
        }

        /// <summary>
        /// 将模型实体保存到数据库
        /// </summary>
        /// <param name="te"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task InsertWithID<TEntity>(TEntity te)
        {
            dynamic Te = te;
            string SQLStatement = $"insert into {Te.TableName} ({Te.Columns}) values ({Te.ID},{Te.InsertString()})";
            await _sql.Execute(SQLStatement);
        }

        public async Task Inserts<TEntity>(IList<TEntity> tes)
        {
            if(tes.Count == 0) return;
            dynamic Tes = tes;
            string insertString = "",tableName = Tes[0].TableName,columnsWithoutID = Tes[0].ColumnsWithoutID;
            for(int i = 1;i < Tes.Count - 1;i++)
            {
                insertString += $"({Tes[i].InsertString()})";
                insertString += ",";
            }
            insertString += $"({Tes[Tes.Count - 1].InsertString()})";
            string SQLStatement = $"insert into {tableName} ({columnsWithoutID}) values {insertString}";
            await _sql.Execute(SQLStatement);
        }

        public async Task InsertsWithID<TEntity>(IList<TEntity> tes)
        {
            if(tes.Count == 0) return;
            dynamic Tes = tes;
            string insertString = "",tableName = Tes[0].TableName,columns = Tes[0].Columns;
            for(int i = 1;i < Tes.Count - 1;i++)
            {
                insertString += $"({Tes[i].ID},{Tes[i].InsertString()})";
                insertString += ",";
            }
            insertString += $"({Tes[Tes.Count - 1].ID},{Tes[Tes.Count - 1].InsertString()})";
            string SQLStatement = $"insert into {tableName} ({columns}) values {insertString}";
            await _sql.Execute(SQLStatement);
        }

        /// <summary>
        /// 将模型实体从数据库移除
        /// </summary>
        /// <param name="te"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task Delete<TEntity>(TEntity te)
        {
            dynamic Te = te;
            string SQLStatement = $"delete from {Te.TableName} where ID = {Te.ID}";
            await _sql.Execute(SQLStatement);
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="tes"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task Deletes<TEntity>(IList<TEntity> tes)
        {
            dynamic Tes = tes;
            string IDs = default;
            for(int i = 0;i < Tes.Count - 1;i++)
            {
                IDs += $"{Tes[i].ID},";
            }
            IDs += $"{Tes[Tes.Count - 1].ID}";
            string SQLStatement = $"delete from {Tes[0].TableName} where ID in ({IDs})";
            await _sql.Execute(SQLStatement);
        }

        /// <summary>
        /// 更新模型实体
        /// </summary>
        /// <param name="te"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task Update<TEntity>(TEntity te)
        {
            dynamic Te = te;
            string SQLStatement = $"update {Te.TableName} set {Te.UpdateString()} where ID = {Te.ID}";
            await _sql.Execute(SQLStatement);
        }

        /// <summary>
        /// 根据ID选择实体
        /// </summary>
        /// <param name="te"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<TEntity> Select<TEntity>(TEntity te)
        {
            dynamic Te = te;
            string SQLStatement = $"select * from {Te.TableName} where ID = {Te.ID}";
            IList<TEntity> result = Te.GetList(await _sql.Query(SQLStatement));
            if(result.Count == 0) return default;
            te = result[0];
            return result[0];
        }

        /// <summary>
        /// 根据实体中的候选码选择ID,并保存在实例中
        /// </summary>
        /// <param name="te"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<long> SelectID<TEntity>(TEntity te)
        {
            dynamic Te = te;
            string SQLStatement = $"select * from {Te.TableName} where {Te.SelectString()}";
            dynamic result = Te.GetList(await _sql.Query(SQLStatement));
            if(result.Count == 0) return long.MinValue;
            Te.ID = result[0].ID;
            return result[0].ID;
        }


        /// <summary>
        /// 根据提供的ID列表选择实体列表
        /// </summary>
        /// <param name="tes"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<IList<TEntity>> Selects<TEntity>(IList<TEntity> tes)
        {
            dynamic Tes = tes;
            string IDs = default;
            for(int i = 0;i < Tes.Count - 1;i++)
            {
                IDs += $"{Tes[i].ID},";
            }
            IDs += $"{Tes[Tes.Count - 1].ID}";
            string SQLStatement = $"select * from {Tes[0].TableName} where ID in ({IDs})";
            IList<TEntity> result = Tes[0].GetList(await _sql.Query(SQLStatement));
            if(result.Count == 0) return new List<TEntity>();
            return result;
        }

        /// <summary>
        /// 根据提供的实体列表选择ID列表
        /// </summary>
        /// <param name="tes"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<List<TEntity>> SelectIDs<TEntity>(IList<TEntity> tes)
        {
            dynamic Tes = tes;
            string constraints = default;
            for(int i = 0; i < Tes.Count - 1;i++)
            {
                constraints += $"({Tes[i].SelectString()})";
                constraints += " or ";
            }
            constraints += $"({Tes[Tes.Count - 1].SelectString()})";
            string SQLStatement = $"select * from {Tes[0].TableName} where {constraints}";
            List<TEntity> result = Tes[0].GetList(await _sql.Query(SQLStatement));
            return result;
        }

        
        /// <summary>
        /// 选择全部实体
        /// </summary>
        /// <param name="exampleInstance"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<IList<TEntity>> SelectAll<TEntity>(TEntity exampleInstance)
        {
            dynamic Te = exampleInstance;
            string SQLStatement = $"select * from {Te.TableName}";
            IList<TEntity> result = Te.GetList(await _sql.Query(SQLStatement));
            if(result.Count == 0) return default;
            return result;
        }


        /// <summary>
        /// 自定义选择实体
        /// !!这个方法不安全,谨慎使用!!
        /// </summary>
        /// <param name="te"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<IList<TEntity>> SelectCustom<TEntity>(TEntity exampleInstance,string selectString)
        {
            dynamic Te = exampleInstance;
            string SQLStatement = $"select * from {Te.TableName} where {selectString}";
            IList<TEntity> result = Te.GetList(await _sql.Query(SQLStatement));
            if(result.Count == 0) return new List<TEntity>();
            return result;
        }

        public async Task<ID_IDList> SelectAllRelations<TKeyEntity,TValueEntity>(TKeyEntity keyExampleInstance,TValueEntity valueExampleInstance)
        {
            dynamic Tke = keyExampleInstance,Tve = valueExampleInstance;
            string tableName = $"{Tke.TableName}_{Tve.TableName}";
            string SQLStatement = $"select * from {tableName}";

            return id_id.GetList(await _sql.Query(SQLStatement));
        }





        /// <summary>
        /// 两个实体之间建立关系
        /// </summary>
        /// <param name="tke"></param>
        /// <param name="tve"></param>
        /// <param name="SetRelations"></param>
        /// <typeparam name="TKeyEntity"></typeparam>
        /// <typeparam name="TValueEntity"></typeparam>
        /// <returns></returns>
        public async Task Connect<TKeyEntity,TValueEntity>(TKeyEntity tke, TValueEntity tve,Action<dynamic> SetRelations)
        {
            ID_ID key_value = new ID_ID();
            dynamic Tke = tke,Tve = tve;
            key_value.ID = Tke.ID;
            key_value.ID_2 = Tve.ID;
            SetRelations(key_value.Relations);
            string tableName = $"{Tke.TableName}_{Tve.TableName}";
            string relationsJSON = JsonConvert.SerializeObject(key_value.Relations);
            string SQLStatement = $"insert into {tableName} values ({Tke.ID},{Tve.ID},'{relationsJSON}')";
            await _sql.Execute(SQLStatement);
        }

        /// <summary>
        /// 两个实体之间解除关系
        /// </summary>
        /// <param name="tke"></param>
        /// <param name="tve"></param>
        /// <typeparam name="TKeyEntity"></typeparam>
        /// <typeparam name="TValueEntity"></typeparam>
        /// <returns></returns>
        public async Task Disconnect<TKeyEntity,TValueEntity>(TKeyEntity tke, TValueEntity tve) 
        {
            dynamic Tke = tke,Tve = tve;
            string tableName = $"{Tke.TableName}_{Tve.TableName}";
            string selectWay = $"{tableName}.{Tke.TableName} = {Tke.ID} and {tableName}.{Tve.TableName} = {Tve.ID}";
            if(tke.GetType() == tve.GetType()) selectWay = $"{tableName}.{Tke.TableName}_1 = {Tke.ID} and {tableName}.{Tve.TableName}_2 = {Tve.ID}";
            string SQLStatement = $"delete from {tableName} where {selectWay}";
            await _sql.Execute(SQLStatement);
        }

        /// <summary>
        /// 根据实体选择已存在的关系
        /// </summary>
        /// <param name="tke"></param>
        /// <param name="tve"></param>
        /// <typeparam name="TKeyEntity"></typeparam>
        /// <typeparam name="TValueEntity"></typeparam>
        /// <returns></returns>
        public async Task<dynamic> SelectRelation<TKeyEntity,TValueEntity>(TKeyEntity tke, TValueEntity tve)
        {
            dynamic Tke = tke,Tve = tve;
            string tableName = $"{Tke.TableName}_{Tve.TableName}";
            string selectWay = $"{tableName}.{Tke.TableName} = {Tke.ID} and {tableName}.{Tve.TableName} = {Tve.ID}";
            if(tke.GetType() == tve.GetType()) selectWay = $"{tableName}.{Tke.TableName}_1 = {Tke.ID} and {tableName}.{Tve.TableName}_2 = {Tve.ID}";
            string SQLStatement = $"select * from {tableName} where {selectWay}";

            ID_IDList resultList = id_id.GetList(await _sql.Query(SQLStatement));
            if(resultList.Count == 0) return default;
            return resultList[0].Relations;
        }


         /// <summary>
        /// 获取两个实体之间的关系,根据关系名
        /// </summary>
        /// <param name="tke"></param>
        /// <param name="tve"></param>
        /// <param name="relationName"></param>
        /// <param name="relationValue"></param>
        /// <typeparam name="TKeyEntity"></typeparam>
        /// <typeparam name="TValueEntity"></typeparam>
        /// <returns></returns>
        public async Task<object> SelectRelation<TKeyEntity, TValueEntity>(TKeyEntity tke, TValueEntity tve, string relationName)
        {
            dynamic relation = await SelectRelation<TKeyEntity,TValueEntity>(tke,tve);
            if(relation == null) return default;
            foreach(var properties in relation)
                if(properties.Key.ToString() == relationName) return properties.Value;
            return default;

        }
        /// <summary>
        /// 改变两个实体间的关系
        /// 注意!在Action<dynamic> SetRelation 中已有relation 的参数,
        /// 不要将别的引用参数的值赋给它,这样会导致修改不到已有的relation.
        /// 正确修改方法 : relation.XX = XX.
        /// 错误修改方法 : realtion = XX.
        /// Action中参数 : 1.dynamic 要改变的关系 2. 返回值 : 是否继续执行
        /// </summary>
        /// <param name="tke"></param>
        /// <param name="tve"></param>
        /// <param name="SetRelations"></param>
        /// <typeparam name="TKeyEntity"></typeparam>
        /// <typeparam name="TValueEntity"></typeparam>
        /// <returns></returns>
        public async Task ChangeRelation<TKeyEntity,TValueEntity>(TKeyEntity tke, TValueEntity tve,Func<dynamic,Task<bool>> SetRelation)
        {
            dynamic Tke = tke,Tve = tve;
            bool IsExecute = true;
            string tableName = $"{Tke.TableName}_{Tve.TableName}";
            dynamic relation = await SelectRelation<TKeyEntity,TValueEntity>(tke,tve);
            IsExecute = await SetRelation(relation);
            if(IsExecute == false) return;
            string relationJSON = JsonConvert.SerializeObject(relation);
            string selectWay = $"{tableName}.{Tke.TableName} = {Tke.ID} and {tableName}.{Tve.TableName} = {Tve.ID}";
            if(tke.GetType() == tve.GetType()) selectWay = $"{tableName}.{Tke.TableName}_1 = {Tke.ID} and {tableName}.{Tve.TableName}_2 = {Tve.ID}";
            string SQLStatement = $"update {tableName} set relations = '{relationJSON}' where {selectWay}";

            await _sql.Execute(SQLStatement);
        }


        /// <summary>
        /// 新增关系
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="relation"></param>
        /// <returns></returns>
        public async Task AppendRelation<TKeyEntity, TValueEntity>(TKeyEntity tke, TValueEntity tve, string relationName, string newRelation)
        {
            await ChangeRelation<TKeyEntity, TValueEntity>(tke, tve, async relation =>
               {
                   if (relation == null)
                   {   //两个实体之间的关系不存在,则新建关系,并取消update执行
                       await Connect<TKeyEntity, TValueEntity>(tke, tve, relation => ((IDictionary<string,Object>)relation).Add(relationName,new List<string>() { newRelation.ToString() }));
                       return false;
                   }
                   foreach (var properties in relation)
                   {
                       //两个实体之间存在名为Type的关系
                       if (properties.Key.ToString() == relationName)
                       {
                            if (!properties.Value.Contains(newRelation.ToString()))
                            {
                                properties.Value.Add(newRelation.ToString());
                            }
                            return true;
                       }
                   }
                   //两个实体之间不存在名为Type的关系
                   ((IDictionary<string,Object>)relation).Add(relationName,new List<string>() { newRelation.ToString() });
                   return true;

               });

        }

        /// <summary>
        /// 移除关系
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="relation"></param>
        /// <returns></returns>
        public async Task RemoveRelation<TKeyEntity, TValueEntity>(TKeyEntity tke, TValueEntity tve,string relationName, string oldRelation)
        {
            await ChangeRelation<TKeyEntity, TValueEntity>(tke, tve, async relation =>
             {
                 //relation.Type.Remove(oldRelation.ToString());
                 foreach (var properties in relation)
                 {
                     if (properties.Key == relationName)
                     {
                         properties.Value.Remove(oldRelation.ToString());
                         if (properties.Value.Count == 0)
                         {
                             await Disconnect<TKeyEntity, TValueEntity>(tke, tve);
                             return false;
                         }
                     }
                 }
                 return true;
             });
        }


       

        /// <summary>
        /// 在数据库中匹配得到输入实体和对应的输出实体
        /// </summary>
        /// <param name="sql">注入的数据库服务</param>
        /// 
        /// <param name="input">输入实体</param>
        /// <param name="outputExampleInstance">输出实体实例样例,输入默认的构造函数即可</param>
        /// <param name="type">输出实体在数据库中关系表中的位置,如在第一个则为Key,在第二个为Value</param>
        /// <typeparam name="TInputEntity">输入实体</typeparam>
        /// <typeparam name="TOutputEntity">输出实体</typeparam>
        /// <returns></returns>
        public async Task<IDictionary<TOutputEntity,dynamic>> Mapping<TInputEntity,TOutputEntity>(TInputEntity input,TOutputEntity outputExampleInstance,ID_IDList.OutPutType type)
        {
            dynamic i = input, o = outputExampleInstance;
            string iTableName = i.TableName,relationTableName = default,relationWay = default,connectedTable = default,SQLStatement = default,iTempTableName = default,oTempTableName = default;

            if(type == ID_IDList.OutPutType.Key) relationTableName = $"{o.TableName}_{i.TableName}";
            else relationTableName = $"{i.TableName}_{o.TableName}";

            connectedTable = $"{o.TableName},{relationTableName},{i.TableName}";

            if(typeof(TInputEntity) == typeof(TOutputEntity))
            {
                iTempTableName = i.TableName;
                oTempTableName = o.TableName;
                if(type == ID_IDList.OutPutType.Value) 
                {
                    i.TableName = i.TableName + "_1";
                    o.TableName = o.TableName + "_2";
                }
                else
                {
                    i.TableName = i.TableName + "_2";
                    o.TableName = o.TableName + "_1";
                }
                connectedTable = $"{oTempTableName} as {o.TableName},{relationTableName},{iTempTableName} as {i.TableName}";
            }

            relationWay = $"{i.TableName}.ID = {relationTableName}.{i.TableName} and {o.TableName}.ID = {relationTableName}.{o.TableName}";

            SQLStatement = $"select {o.Columns},relations from {connectedTable} where {relationWay} and {i.TableName}.ID = {i.ID}";

            i.TableName = iTableName;

            return o.GetRelationDictionary(await _sql.Query(SQLStatement));

        }

        /// <summary>
        /// 在数据库中匹配得到输入实体和对应的输出实体,并筛选出符合关系条件的实体
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputExampleInstance"></param>
        /// <param name="type"></param>
        /// <param name="selections"></param>
        /// <typeparam name="TInputEntity"></typeparam>
        /// <typeparam name="TOutputEntity"></typeparam>
        /// <returns></returns>
        public async Task<IList<TOutputEntity>> MappingSelect<TInputEntity,TOutputEntity>(TInputEntity input,TOutputEntity outputExampleInstance,ID_IDList.OutPutType type,Action<dynamic> SetSelections)
        {
            IDictionary<TOutputEntity,dynamic> mappingResults = await Mapping<TInputEntity,TOutputEntity>(input,outputExampleInstance,type);
            IList<TOutputEntity> mappingResultsSelect = new List<TOutputEntity>();
            bool selectionFlag = default, flag = default,itemFlag = default;
            ID_ID key_value = new ID_ID();
            dynamic selections = key_value.Relations;
            SetSelections(selections);
            foreach(var mappingResult in mappingResults)
            {
                flag = true;
                foreach(var selection in selections)
                {
                    selectionFlag = false;
                    if(mappingResult.Value == null) break;
                    foreach(var relation in mappingResult.Value)
                    {
                        if(relation.Key == selection.Key) 
                        {
                            //如果两个值都是集合,判断selection.value是不是relation.value的真子集
                            
                            if(typeof(ICollection<object>).IsAssignableFrom(relation.Value.GetType()))
                            {
                                Type ICollectionType = typeof(ICollection<object>).GetGenericTypeDefinition();
                                if(!Array.Exists(selection.Value.GetType().GetInterfaces(), new Predicate<Type>(type => type.GetGenericTypeDefinition() == ICollectionType))) break;
                                itemFlag = true;
                                foreach(var selectionItem in selection.Value)
                                {
                                    if(!relation.Value.Exists(new Predicate<dynamic>(relationItem => relationItem == selectionItem)))
                                    {
                                        itemFlag = false;
                                        break;
                                    }
                                }
                                if(itemFlag == true)
                                {
                                    selectionFlag = true;//如果匹配到了,直接进行下一个selection的判断
                                    break;
                                }

                            }
                            else if(relation.Value == selection.Value)
                            {
                                selectionFlag = true;//如果匹配到了,直接进行下一个selection的判断
                                break;
                            }
                            
                        }
                    }
                    if(selectionFlag == false)//有一个selection没匹配到
                    {
                        flag = false;
                        break;//如果某一个selection没有找到
                    }
                    //...全部匹配到
                }
                if(flag == true) //所有selection满足要求      
                    mappingResultsSelect.Add(mappingResult.Key);
            }
            return mappingResultsSelect;
        }


        /// <summary>
        /// 查找所有关系的并集,并使用字典统计每一个输出实体与输入实体的所有关系
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Key_ListValue_Pairs<TOutputEntity,KeyValuePair<TInputEntity, dynamic>>> MappingUnionStatistics<TInputEntity,TOutputEntity>(IList<TInputEntity> inputs,TOutputEntity outputExampleInstance,ID_IDList.OutPutType type)
        {
            //以某些标签能够匹配到的其他用户为例
            Key_ListValue_Pairs<TOutputEntity,KeyValuePair<TInputEntity, dynamic>> dictionary = new Key_ListValue_Pairs<TOutputEntity,KeyValuePair<TInputEntity, dynamic>>();
            if (inputs.Count == 0) return dictionary;

            foreach(var input in inputs)//每一个input是一个标签的
            {
                IDictionary<TOutputEntity,dynamic> targets = await Mapping<TInputEntity,TOutputEntity>(input,outputExampleInstance,type);//找到拥有该标签的用户
                foreach(var target in targets)//每一个target是一个用户的id
                {
                    int index = dictionary.KeyIndex(target.Key);
                    KeyValuePair<TInputEntity, dynamic> inputEntity_Relation = new KeyValuePair<TInputEntity, dynamic>(input,target.Value);
                    
                    if(index != int.MinValue)//如果字典中存在该帖子,则添加拥有该帖子的用户
                        dictionary[index].Value.Add(inputEntity_Relation);

                    else//如果不存在,则新建一个拥有该帖子的用户list
                        dictionary.Insert(0,new KeyValuePair<TOutputEntity, List<KeyValuePair<TInputEntity, dynamic>>>(target.Key,new List<KeyValuePair<TInputEntity, dynamic>>(){inputEntity_Relation}));
                }
            }
            return dictionary;
        }


        /// <summary>
        /// 查找所有关系的并集,并使用字典统计每一个输出实体与输入实体的所有关系,根据输入条件筛选符合条件的关系
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Key_ListValue_Pairs<TOutputEntity,TInputEntity>> MappingSelectUnionStatistics<TInputEntity,TOutputEntity>(IList<TInputEntity> inputs,TOutputEntity outputExampleInstance,ID_IDList.OutPutType type,Action<dynamic> SetSelections)
        {
            //以某些标签能够匹配到的其他用户为例
            Key_ListValue_Pairs<TOutputEntity,TInputEntity> dictionary = new Key_ListValue_Pairs<TOutputEntity,TInputEntity>();
            if (inputs.Count == 0) return dictionary;
            ID_ID key_value = new ID_ID();
            dynamic selections = key_value.Relations;

            foreach(var input in inputs)//每一个input是一个标签的
            {
                IList<TOutputEntity> targets = await MappingSelect<TInputEntity,TOutputEntity>(input,outputExampleInstance,type,SetSelections);//找到拥有该标签的用户
                foreach(var target in targets)//每一个target是一个用户的id
                {
                    int index = dictionary.KeyIndex(target);
                    
                    if(index != int.MinValue)//如果字典中存在该帖子,则添加拥有该帖子的用户
                        dictionary[index].Value.Add(input);

                    else//如果不存在,则新建一个拥有该帖子的用户list
                        dictionary.Insert(0,new KeyValuePair<TOutputEntity, List<TInputEntity>>(target,new List<TInputEntity>(){input}));
                }
            }
            return dictionary;
        }

    }

}
