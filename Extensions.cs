namespace yt6983138.github.io
{
	public static class Extensions
	{
		public static void MoveItemAtIndexToFront<T>(this List<T> list, int index)
		{
			T item = list[index];
			for (int i = index; i > 0; i--)
				list[i] = list[i - 1];
			list[0] = item;
		}
	}
}
