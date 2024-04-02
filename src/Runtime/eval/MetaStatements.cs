using metsu.src.Runtime.eval.nativeModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math = metsu.src.Runtime.eval.nativeModules.Math;

namespace NewLangInterpreter.Runtime.eval
{
    internal static class MetaStatements
    {
        public static Values.NullVal eval_meta_mutable_default(bool is_constant, Environment env)
        {
            env.set_default_const(is_constant);
            return new Values.NullVal();
        }

        public static Values.NullVal eval_meta_silly_default(bool is_silly, Environment env)
        {
            env.is_silly = is_silly;
            return new Values.NullVal();
        }

        public static Values.NullVal eval_meta_include(string path, string alias, Environment env)
        {
            if (native_modules.ContainsKey(path))
            {
                Values.ObjectVal module = native_modules[path]();

                env.declareVar(alias, module, true, src.Frontend.AST.DataType.Object);
                return new Values.NullVal();
            }
            else
            {
                throw new Exception("Language does not yet support custom modules");
            }
            
        }

        private static Dictionary<string, Func<Values.ObjectVal>> native_modules = new Dictionary<string, Func<Values.ObjectVal>>()
        {
            { "io", Io.getModule },
            { "math", Math.getModule },
            { "chrono", Chrono.getModule },
        };
        
    }
}
