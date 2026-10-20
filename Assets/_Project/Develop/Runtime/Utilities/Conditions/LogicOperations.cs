namespace Assets._Project.Develop.Runtime.Utilities.Conditions
{

    public partial class CompositeCondition
    {
        public class LogicOperations
        {
            public static bool And(bool a, bool b) => a && b;
            public static bool Or(bool a, bool b) => a || b;
        }
    }
}
