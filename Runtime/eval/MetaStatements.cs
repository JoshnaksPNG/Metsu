using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
