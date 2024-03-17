namespace FabricCutter.UI.Logic
{
	public class MarkerFactory : IMarkerFactory
	{
		private readonly IMarkerFactoryValidator _markerFactoryValidator;

		private Marker _currentMarker { get; set; }

        public MarkerFactory(IMarkerFactoryValidator markerFactoryValidator)
        {
			this._markerFactoryValidator = markerFactoryValidator;
		}


        public MarkerBase Build()
		{
			_markerFactoryValidator.Validate(_currentMarker);
			if (!_markerFactoryValidator.IsValid)
			{
				return new MarkerInvalid();
			}

			return _currentMarker;
		}

		public bool WithStartMarkerPosition(int newMarkerId, int startPosition)
		{
			_currentMarker = new Marker(newMarkerId, startPosition, int.MinValue);
			return true;
		}
		public bool WithStopMarkerPosition(int endPosition)
		{
			if (_currentMarker is null)
				return false;
			if (_currentMarker.EndPosition <= _currentMarker.StartPosition)
				return false;
			_currentMarker.EndPosition = endPosition;
			return true;
		}

		public bool WithStartSubMarkerPosition(int startPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null)
				return false;
			_currentMarker.SubMarker = new SubMarker(_currentMarker.Id,startPosition, int.MinValue);
			return true;
		}
		public bool WithStopSubMarkerPosition(int endPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null)
				return false;

			if (_currentMarker.SubMarker.EndPosition <= _currentMarker.SubMarker.StartPosition)
				return false;
			_currentMarker.EndPosition = endPosition;
			return true;
		}



	}

	public class MarkerFactoryValidator : IMarkerFactoryValidator
	{

		public bool IsValid { get; set; }
		public bool IsStartMarkerEnable { get; set; }
		public bool IsEndMarkerEnable { get; set; }
		public bool IsStartSubMarkerEnable { get; set; }
		public bool IsEndSubMarkerEnable { get; set; }

		public MarkerFactoryValidator Validate(Marker marker)
		{

			return this;
		}

	}
}
