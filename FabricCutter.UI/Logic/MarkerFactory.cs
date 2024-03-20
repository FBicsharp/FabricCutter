namespace FabricCutter.UI.Logic
{
	public class MarkerFactory : IMarkerFactory
	{
		private Marker _currentMarker { get; set; }		


		public void Clear()
		{
			_currentMarker = null;
		}

		public Marker BuildMarker()
		{
			return _currentMarker;
		}

		public void WithStartMarkerPosition(int newMarkerId, int startPosition)
		{
			if (_currentMarker is null)//fino a quando non finisco di creare il marker ottengo quello corrente
				_currentMarker = new Marker(newMarkerId, startPosition, int.MinValue);
			return;
		}
		public void WithStopMarkerPosition(int endPosition)
		{
			if (_currentMarker is null)
				return;
			if (endPosition <= _currentMarker.StartPosition)
				return;
			_currentMarker.EndPosition = endPosition;
		}

		public void WithStartSubMarkerPosition(int startPosition)
		{
			_currentMarker.SubMarker ??= new List<SubMarker>();
			var thereIsAnyIncompleteSubMarker = _currentMarker.SubMarker.Where(x => x.EndPosition == int.MinValue).FirstOrDefault();
			if (thereIsAnyIncompleteSubMarker is null)
				_currentMarker.SubMarker.Add(new SubMarker(_currentMarker.Id, startPosition, int.MinValue));
			else
				thereIsAnyIncompleteSubMarker.StartPosition = startPosition;
		}
		public void WithStopSubMarkerPosition(int endPosition)
		{
			if (endPosition <= _currentMarker.StartPosition || _currentMarker.SubMarker is null)
				return;
			var existingSubMarker = _currentMarker.SubMarker.Where(x => x.EndPosition == int.MinValue).FirstOrDefault();
			if (existingSubMarker is null)
				return;
			existingSubMarker.EndPosition = endPosition;
		}



	}


}

