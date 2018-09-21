using System;
using System.Collections.Generic;
using ZCSharpLib;
using ZCSharpLib.Common.Provider;
using ZCSharpLib.Features.Model;
using ZCSharpLib.ZSupport.Adapter;
using ZCSharpLib.ZTUtils;

public class ModelProvider : IZServiceProvider
{
    private List<ModelObject> Models
    {
        get;
        set;
    }

    private IModelAccesser Accesser
    {
        get;
        set;
    }

    public void Register()
    {
        Type[] types = ZCommUtil.GetTypes();
        foreach (Type type in types)
        {
            if (type.IsClass && type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(BaseModel<>))
            {
                ModelObject item = App.Singleton(type) as ModelObject;
                Models.Add(item);
            }
        }
        if (ModelSettings.IsSaveDB)
        {
            Accesser = new ModelAccesser();
            for (int j = 0; j < Models.Count; j++)
            {
                ModelObject modelObject = Models[j];
                modelObject.SetAccesser(Accesser);
            }
        }
    }

    public void Initialize()
    {
    }

    public void Uninitialize()
    {
    }

    public void Unregister()
    {
    }
}