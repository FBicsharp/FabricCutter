namespace FabricCutter.UI.Logic
{
	public class MarkerFactory : IMarkerFactory
	{
		private Marker _currentMarker { get; set; }


		public void Clear()
		{
			_currentMarker = null;
		}

		public MarkerBase Build()
		{
			//_markerFactoryValidator.Validate(_currentMarker);
			//if (!_markerFactoryValidator.IsValid)
			//{
			//	return new MarkerInvalid();
			//}
			var result = _currentMarker;
			Clear();

			return result;
		}

		public Marker WithStartMarkerPosition(int newMarkerId, int startPosition)
		{
			if (_currentMarker is null)//fino a quando non finisco di creare il marker ottengo quello corrente
				_currentMarker = new Marker(newMarkerId, startPosition, int.MinValue);
			return _currentMarker;
		}
		public Marker WithStopMarkerPosition(int endPosition)
		{
			if (_currentMarker is null)
				return _currentMarker;
			if (endPosition <= _currentMarker.StartPosition)
				return _currentMarker;
			_currentMarker.EndPosition = endPosition;
			return _currentMarker;
		}

		public Marker WithStartSubMarkerPosition(int startPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null)
				return _currentMarker;
			_currentMarker.SubMarker = new SubMarker(_currentMarker.Id, startPosition, int.MinValue);
			return _currentMarker;
		}
		public Marker WithStopSubMarkerPosition(int endPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null)
				return _currentMarker;

			if (_currentMarker.SubMarker.EndPosition <= _currentMarker.SubMarker.StartPosition)
				return _currentMarker;
			_currentMarker.EndPosition = endPosition;
			return _currentMarker;
		}



	}


}
