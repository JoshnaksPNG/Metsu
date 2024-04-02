using NewLangInterpreter.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metsu.src.Runtime.eval.nativeModules
{
    internal interface NativeModule
    {
        public static Values.ObjectVal getModule() { return new Values.ObjectVal(); }
    }
}
