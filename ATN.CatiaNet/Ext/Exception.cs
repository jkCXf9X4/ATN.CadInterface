using System;

namespace ATN.Catia.R24.Ext
{

	public class CatiaException : Exception
	{
		
		public CatiaException()
		{
		}

		public CatiaException(string message)
			: base(message)
		{
			// Add implementation.
		}
	}
	
		
	public class InvalidPrerequisites: CatiaException
	{
		public InvalidPrerequisites()
		{
		}
		
		public InvalidPrerequisites(string message)
			: base(message)
		{
			// Add implementation.
		}

	}
	
	
	public class NoSelectionException : CatiaException
	{
		public NoSelectionException()
		{
		}
		
		public NoSelectionException(string message)
			: base(message)
		{
			// Add implementation.
		}

	}
	
	public class MultipleSelectionException : CatiaException
	{
		public MultipleSelectionException()
		{
		}
		
		public MultipleSelectionException(string message)
			: base(message)
		{
			// Add implementation.
		}

	}
	
	public class NotFoundException : CatiaException // 
	{
		public NotFoundException()
		{
		}
 
		public NotFoundException(string message)
			: base(message)
		{
			// Add implementation.
		}
	}
	
	public class MultipleFoundException : CatiaException // 
	{
		public MultipleFoundException()
		{
		}
 
		public MultipleFoundException(string message)
			: base(message)
		{
			// Add implementation.
		}
	}
}
