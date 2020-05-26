using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

public delegate void WarningResult();
public class WarningModel
{
    public WarningResult result;
    public string value;

    public WarningModel (string value , WarningResult result=null)
    {
        this.value = value;
        this.result = result;
     }

}
