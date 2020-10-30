using Templates.Tasks;

namespace Templates.Tasks.Messages
{
  // an abstract message
  public abstract class Message : TemplateTask
  { 
    // implement messages as decorator pattern
    public Message() : base(TaskType.Message) {}
  }
}