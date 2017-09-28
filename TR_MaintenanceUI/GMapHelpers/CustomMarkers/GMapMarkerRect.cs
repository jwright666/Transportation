
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
   public class GMapMarkerRect : GMapMarker, ISerializable
   {
      [NonSerialized]
      public Pen Pen;

      [NonSerialized]
      private int m_Radius;

      [NonSerialized]
      private int m_Length;

      [NonSerialized]
      private Rectangle m_LocalRectangle;

      private Size m_RecSize;

      [NonSerialized]
      public GMarkerGoogle InnerMarker;

      [NonSerialized]
      private string m_DefaultToolTip;

      [NonSerialized]
      private string m_TransportToolTip;

      [NonSerialized]
      private string m_DurationToolTip;

      public GMapMarkerRect(PointLatLng p)
         : base(p)
      {
         Pen = new Pen(Brushes.Blue, 5);

         // do not forget set Size of the marker
         // if so, you shall have no event on it ;}
         //Size = new System.Drawing.Size(R, R);
         m_Radius = 888;
         Size = new System.Drawing.Size(30, 30);
         //Offset = new System.Drawing.Point(Size.Width / 2, Size.Height / 2);
         //Offset = new System.Drawing.Point(-100, -100);
          //Size = new System.Drawing.Size(111, 111);
          //Offset = new System.Drawing.Point(10, 10);
      }

      public override void OnRender(Graphics g)
      {
          //g.DrawRectangle(Pen, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height));
          int R = (int)((m_Radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;
          m_Length = R;
          m_RecSize = new System.Drawing.Size(R, R);
          g.DrawRectangle(Pen, new System.Drawing.Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));
          m_LocalRectangle = new Rectangle();
          m_LocalRectangle.Location = new Point(LocalPosition.X - R / 2, LocalPosition.Y - R / 2);
          m_LocalRectangle.Size = m_RecSize;
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

       public int RectangleLength
       {
           get
           {
               return m_Length;
           }
           set
           {
               m_Length = value;
           }
       }

       public Size RectangleSize
       {
           get
           {
               return m_RecSize;
           }
           set
           {
               m_RecSize = value;
           }
       }

       public Rectangle LocalRectangle
       {
           get
           {
               return m_LocalRectangle;
           }
           set
           {
               m_LocalRectangle = value;
           }
       }
       public string DefaultToolTip
       {
           get
           {
               return m_DefaultToolTip;
           }
           set
           {
               m_DefaultToolTip = value;
           }
       }

       public string TransportToolTip
       {
           get
           {
               return m_TransportToolTip;
           }
           set
           {
               m_TransportToolTip = value;
           }
       }

       public string DurationToolTip
       {
           get
           {
               return m_DurationToolTip;
           }
           set
           {
               m_DurationToolTip = value;
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

      protected GMapMarkerRect(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      #endregion
   }
}
