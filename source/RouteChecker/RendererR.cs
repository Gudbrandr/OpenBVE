// ╔═════════════════════════════════════════════════════════════╗
// ║ Renderer.cs for the Route Checker                           ║
// ╠═════════════════════════════════════════════════════════════╣
// ║ This file cannot be used in the openBVE main program.       ║
// ║ The file from the openBVE main program cannot be used here. ║
// ╚═════════════════════════════════════════════════════════════╝

using System;

namespace OpenBve {
	internal static partial class Renderer {

		// first frame behavior
		internal enum LoadTextureImmediatelyMode { NotYet, Yes, NoLonger }
		internal static LoadTextureImmediatelyMode LoadTexturesImmediately = LoadTextureImmediatelyMode.NotYet;

	}
}