Get 根据本表里的外键寻找 关系为一对一 -- **0**  
Select 根据关系表里的外键 关系为一对多/多对一/多对多 -- **1**  
Mapping 为多次的Select,将Select出的结果作为条件再次Select,出现Union -- **2**  
0 ==> Get  
1 ==> Select  
\>1 ==> Mapping  