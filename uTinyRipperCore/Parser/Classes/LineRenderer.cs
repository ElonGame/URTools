using System.Collections.Generic;
using uTinyRipper.Classes.TrailRenderers;
using uTinyRipper.Classes.Renderers;
using uTinyRipper.Converters;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes
{
	public sealed class LineRenderer : Renderer
	{
		public LineRenderer(AssetInfo assetInfo) :
			base(assetInfo)
		{
		}

		/// <summary>
		/// 5.0.0 and greater
		/// </summary>
		public static bool HasLoop(Version version) => version.IsGreaterEqual(5);

		public override void Read(AssetReader reader)
		{
			base.Read(reader);

			Positions = reader.ReadAssetArray<Vector3f>();
			Parameters.Read(reader);
			UseWorldSpace = reader.ReadBoolean();
			if (HasLoop(reader.Version))
			{
				Loop = reader.ReadBoolean();
			}
		}

		public override void Write(AssetWriter writer)
		{
			base.Write(writer);

			Positions.Write(writer);
			Parameters.Write(writer);
			writer.Write(UseWorldSpace);
			if (HasLoop(writer.Version))
			{
				writer.Write(Loop);
			}
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);
			node.InsertSerializedVersion(1);
			node.Add("m_Positions", Positions.ExportYAML(container));
			node.Add("m_Parameters", Parameters.ExportYAML(container));
			node.Add("m_UseWorldSpace", UseWorldSpace);
			if (HasLoop(container.ExportVersion))
			{
				node.Add("m_Loop", Loop);
			}

			return node;
		}

		public Vector3f[] Positions { get; set; }
		public LineParameters Parameters = new LineParameters();
		public bool UseWorldSpace { get; set; }
		public bool Loop { get; set; }
	}
}
