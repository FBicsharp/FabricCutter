namespace FabricCutter.UI.Logic
{
	public class MarkerFactory
	{
        
		private Marker _currentMarker { get; set; }		



		public Marker Build()
		{
			return _currentMarker;
		}

		public void WithStartMarkerPosition(int newMarkerId, int startPosition)
		{
			_currentMarker = new Marker(newMarkerId, startPosition, int.MinValue);
		}
		public void WithStopMarkerPosition(int endPosition)
		{
			if (_currentMarker is null  )
				return;
			if (_currentMarker.EndPosition<= _currentMarker.StartPosition)
				return;
			_currentMarker.EndPosition = endPosition;
		}

		public void WithStartSubMarkerPosition( int startPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null )
				return;
			_currentMarker.SubMarker = new SubMarker(startPosition, int.MinValue);
		}
		public void WithStopStopMarkerPosition(int endPosition)
		{
			if (_currentMarker is null || _currentMarker.SubMarker is null)
				return;			

			if (_currentMarker.SubMarker.EndPosition <= _currentMarker.SubMarker.StartPosition)
				return;
			_currentMarker.EndPosition = endPosition;
		}



	}
}
