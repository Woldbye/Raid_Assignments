using System.Collections.Generic;

namespace Generics
{
  // TO:DO MAKE GENERIC INTERFACE
  public abstract class GenericContext<TInput, TOutput>
  {
    protected TInput _input;
    protected TOutput _output;

    public GenericContext(TInput input)
    {
      this._input = input;
      this.InitializeOutput();
    }

    protected abstract void InitializeOutput();
  } 
}