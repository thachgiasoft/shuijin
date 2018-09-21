using System;
using System.Collections.Generic;
using ZCSharpLib.Common;
using ZCSharpLib.ZTUtils;

namespace ZCSharpLib.Features.Model
{
    public class ModelSetup
    {
        private List<ModelObject> Models { get; set; }

        public ModelSetup()
        {
            Models = new List<ModelObject>();
            Type[] oTypes = ZCommUtil.GetTypes();
            for (int i = 0; i < oTypes.Length; i++)
            {
                Type type = oTypes[i];
                if (type.IsClass && type.BaseType != null && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(BaseModel<>))
                {
                    ModelObject obj = App.Singleton(type) as ModelObject;
                    Models.Add(obj);
                }
            }
        }

        public void Settings(IModelAccesser settings)
        {
            if (settings != null)
            {
                for (int i = 0; i < Models.Count; i++)
                {
                    ModelObject model = Models[i];
                    model.InjectData(settings);
                }
            }
        }

        public void Setup(List<List<BaseData>> oDataList)
        {
            for (int i = 0; i < oDataList.Count; i++)
            {
                List<BaseData> oDatas = oDataList[i];
                if (oDatas.Count > 0)
                {
                    BaseData oData = oDatas[0];
                    Type tType = oData.GetType();
                    ModelObject model = FindModelByDataType(tType);
                    if (model != null) { model.InjectData(oDatas); }
                    else { ZLogger.Error("当前数据类型 type={0} 没有找到对应的Model", tType); }
                }
            }
        }

        private ModelObject FindModelByDataType(Type type)
        {
            ModelObject model = Models.Find((t) => { return t.DataType == type; });
            return model;
        }
    }
}
