namespace Templates.Tasks
{
  // abstract expression
  public abstract class TemplateExpression
  {
    protected TemplateTaskContext _context;

    public TemplateExpression(TemplateTaskContext context = null)
    {
      this._context = context;
    }
    
    public TemplateTaskContext Context 
    { 
      get { return this._context; }
      set { this._context = value; }
    }

    public abstract bool Interpret();

    public virtual bool IsContextSet()
    {
      if (this._context == null)
      {
        return false;
      }
      return true;
    }
  } 
}