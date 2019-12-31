namespace FZeroGXEditor.Serialization
{
	public class Triangle : IBinarySerializable
	{
		public void Serialize(FZWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public static Triangle Deserialize(FZReader reader)
		{
			var obj = new Triangle();

			return obj;
		}
	}
}
