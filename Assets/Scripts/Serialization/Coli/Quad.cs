namespace FZeroGXEditor.Serialization
{
	public class Quad : IBinarySerializable
	{
		public void Serialize(FZWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public static Quad Deserialize(FZReader reader)
		{
			var obj = new Quad();

			return obj;
		}
	}
}
