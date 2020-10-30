using Generics;
using System;

namespace Templates.Tasks
{
  public abstract class DecoratedTask<TDecoration> : TemplateTask where TDecoration : struct, IConvertible
  {
    protected static bool IsReady;
    private static GenericFlags<TDecoration> _flags = null;
    private static GenericInputInfo<TDecoration> _info = null;
    private TemplateTask _task;
    protected object[] Decorations;

    static DecoratedTask()
    {
      DecoratedTask<TDecoration>.IsReady = false;
      DecoratedTask<TDecoration>.Flags = null;
      DecoratedTask<TDecoration>.Info = null;
    }

    public DecoratedTask(TemplateTask task) : base(task.GetTaskType())
    {
      this._task = task;
      int count = Enum.GetValues(typeof(TDecoration)).Length;
      this.Decorations = new object[count];
      if (!DecoratedTask<TDecoration>.IsReady && 
          (DecoratedTask<TDecoration>.Flags != null) && 
          (DecoratedTask<TDecoration>.Info != null))
      {
        DecoratedTask<TDecoration>.IsReady = true;
      }
    }

    public DecoratedTask(TemplateTask task, object[] previousDecorations) : base(task.GetTaskType())
    {
      this._task = task;
      this.Decorations = previousDecorations;
      if (!DecoratedTask<TDecoration>.IsReady && 
          (DecoratedTask<TDecoration>.Flags != null) && 
          (DecoratedTask<TDecoration>.Info != null))
      {
        DecoratedTask<TDecoration>.IsReady = true;
      }
    }

    new public static GenericFlags<TDecoration> Flags
    {
      get { return DecoratedTask<TDecoration>._flags; }
      /*protected*/ set {DecoratedTask<TDecoration>._flags = value;}
    }

    public static GenericInputInfo<TDecoration> Info
    {
      get { return DecoratedTask<TDecoration>._info; }
      /*protected*/ set {DecoratedTask<TDecoration>._info = value;}
    }

    public TemplateTask Task
    {
      get { return this._task; }
      protected set { this._task = value; }
    }


    public object this[TDecoration decoration]
    {
      get { return this.Decorations[Convert.ToInt32(decoration)]; }
      set
      {
        if (value == null | 
            value.GetType().Equals(DecoratedTask<TDecoration>.Info[decoration]))
        {
          this.Decorations[Convert.ToInt32(decoration)] = value;
        }
      }
    }
  
    public bool Contains(params TDecoration[] decorations)
    {
      bool contains = true;
      foreach (TDecoration decoration in decorations)
      {
        contains &= (this[decoration] != null);
      }
      return contains;
    }

    public override string ToRaw()
    {
      return this._task.ToRaw();
    }
  }
}
