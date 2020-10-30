namespace Templates.Tasks
{
  public interface ITemplateInterpreter
  {
    TemplateTask Interpret(string raw); 
  }
}