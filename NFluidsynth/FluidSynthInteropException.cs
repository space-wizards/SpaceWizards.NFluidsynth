using System;

namespace NFluidsynth;

public class FluidSynthInteropException(string message) : Exception(message)
{
  public FluidSynthInteropException() : this("Fluidsynth native error")
  {
  }
}