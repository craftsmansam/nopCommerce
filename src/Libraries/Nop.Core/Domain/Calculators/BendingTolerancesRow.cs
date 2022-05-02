namespace Nop.Core.Domain.Calculators
{
    public partial class BendingTolerancesRow
	{
		public string ArcLength { get; }
		public string PlumbToRadiusLine { get; }
		public string VerticalFace { get; }
		public string PitchinDegrees { get; }
		public string HeightAtMidpoint { get; }
		public string HeightAtTop { get; }

		public BendingTolerancesRow(string arcLength, string plumbToRadiusLine, string verticalFace, string pitchInDegrees,
		                            string heightAtMidpoint, string heightAtTop)
		{
			ArcLength = arcLength;
			PlumbToRadiusLine = plumbToRadiusLine;
			VerticalFace = verticalFace;
			PitchinDegrees = pitchInDegrees;
			HeightAtMidpoint = heightAtMidpoint;
			HeightAtTop = heightAtTop;
		}
	}
}