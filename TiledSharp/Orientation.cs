using System;

namespace TiledSharp
{
	public enum Orientation : byte
	{
		Unknown = 0x00,
		Orthogonal = 0x01,
		Isometric = 0x02,
		Hexagonal = 0x03,
		Shifted = 0x04,
	}
}
