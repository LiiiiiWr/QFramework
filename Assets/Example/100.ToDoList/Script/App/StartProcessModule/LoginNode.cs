using QFramework;
using SCFramework;

namespace ToDoList
{
    public class LoginNode : ExecuteNode
    {
        public override void OnExecute()
        {
            Log.i("ExecuteNode:" + GetType().Name);
            Finish = true;
        }
    }
}
