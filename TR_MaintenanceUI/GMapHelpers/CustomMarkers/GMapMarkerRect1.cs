
namespace FM.TR_MaintenanceUI.GMapHelpers.CustomMarkers
{
   using System.Drawing;
   using GMap.NET.WindowsForms;
   using GMap.NET.WindowsForms.Markers;
   using GMap.NET;
   using System;
   using System.Runtime.Serialization;
   using System.Drawing.Drawing2D;

   [Serializable]
   public class GMapMarkerRect1 : GMapMarker, ISerializable
   {
      [NonSerialized]
       public Pen Pen;

      [NonSerialized]
      public int m_Radius;

      [NonSerialized]
      public GMarkerGoogle InnerMarker;
      /// <summary>
      /// is filled
      /// </summary>
      public bool IsFilled = true;

      /// <summary>
      /// background color
      /// </summary>
      [NonSerialized]
#if !PocketPC
      public Brush Fill = new SolidBrush(Color.FromArgb(155, Color.AliceBlue));
#else
      public Brush Fill = new System.Drawing.SolidBrush(Color.AliceBlue);
#endif

      public GMapMarkerRect1(PointLatLng p)
         : base(p)
      {
         Pen = new Pen(Brushes.Blue, 5);

         // do not forget set Size of the marker
         // if so, you shall have no event on it ;}
         m_Radius = 3000;
         int R = (int)((Radius) / 351.6265) * 2;
         Size = new System.Drawing.Size(R, R);
         Offset = new System.Drawing.Point(-Size.Width / 2, -Size.Height / 2);
          //Size = new System.Drawing.Size(111, 111);
          //Offset = new System.Drawing.Point(10, 10);
      }

      public override void OnRender(Graphics g)
      {
          //g.DrawRectangle(Pen, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height));
          //int Radius = 4000;
          //int R = (int)((Radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;
          //g.DrawEllipse(Pen, new System.Drawing.Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));

          int R = (int)((m_Radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;
          //g.DrawRectangle(Pen, new System.Drawing.Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));

          if (IsFilled)
          {
              g.FillEllipse(Fill, new System.Drawing.Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));
          }
          g.DrawEllipse(Pen, new System.Drawing.Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));
      }

      public int Radius
      {
          get
          {
              return m_Radius;
          }
          set
          {
              m_Radius = value;
          }
      }

      public void Resize()
      {

      }
      public override void Dispose()
      {
         if(Pen != null)
         {
            Pen.Dispose();
            Pen = null;
         }

         if(InnerMarker != null)
         {
            InnerMarker.Dispose();
            InnerMarker = null;
         }

         base.Dispose();
      }

      #region ISerializable Members

      void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
      }

      protected GMapMarkerRect1(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      #endregion
   }
}
