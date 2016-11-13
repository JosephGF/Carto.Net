﻿using System;

namespace NetCarto.Map.WinForms.Designer
{
    public interface ITConvertSerializer
    {
        string Serialize(Object data);
        object Deserialize(string data);
        object Initialize(object current);
    }
}