// Guids.cs
// MUST match guids.h
using System;

namespace Spiral.TfsEssentials
{
    static class GuidList
    {
        public const string guidTfsEssentialsPkgString = "ae932517-901f-49e7-b5b6-c7086cf9276d";
		public const string guidTfsEssentialsCmdSetString = "90765a6a-1d4d-4a49-92de-47df36ebd51f";

        public static readonly Guid guidTfsEssentialsCmdSet = new Guid(guidTfsEssentialsCmdSetString);

		public static Guid TfsProviderGuid = new Guid("4CA58AB2-18FA-4F8D-95D4-32DDF27D184C");
		public static Guid GitProviderGuid = new Guid("11b8e6d7-c08b-4385-b321-321078cdd1f8");
    };
}