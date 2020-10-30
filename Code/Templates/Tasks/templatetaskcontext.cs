using System;
using System.IO;
using System.Collections.Generic;
using Templates.Tasks;
using System.Collections;
using Generics;
using System.Linq;

namespace Templates.Tasks
{ 
  // Template context
  public class TemplateTaskContext : GenericContext<Queue<string>, TemplateTask>
  {
    public TemplateTaskContext(Queue<string> input) : base(input) {}

    // called from base constructor
    protected override void InitializeOutput()
    {
      base._output = null;
    }

    // input
    public Queue<string> Input
    {
      get { return base._input; }
      set { base._input = value; }
    }

    public TemplateTask Output
    {
      get { return base._output; }
      set { base._output = value; }
    }
  }
}