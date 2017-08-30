﻿
using System;
using System.Linq;
using Carto.Core;
using Carto.Layers;
using Carto.Ui;
using CoreGraphics;
using UIKit;

namespace data.collection.iOS
{
    public class ImageEntry : BaseEntry
    {
        public string ImageName { get; set; }

        public UIImageView Photo { get; private set; }

        UIImageView image;

        public ImageEntry(string title, string resource) : base(title)
        {
            Photo = new UIImageView();
			Photo.ClipsToBounds = true;
            Photo.ContentMode = UIViewContentMode.ScaleAspectFill;
            AddSubview(Photo);

            image = new UIImageView();
            image.Image = UIImage.FromFile(resource);
			image.ClipsToBounds = true;
			image.ContentMode = UIViewContentMode.ScaleAspectFit;
            AddSubview(image);

            BringSubviewToFront(label);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Photo.Frame = Bounds;

            nfloat w = Frame.Height / 3.5f;
            nfloat h = w;
            nfloat x = Frame.Width / 2 - w / 2;
            nfloat y = Frame.Height / 2 - h / 2;

			image.Frame = new CGRect(x, y, w, h);
        }

        UIView imageBackground;

        void SetIconRoundWithBackground()
        {
            if (imageBackground != null)
            {
                return;
            }

            imageBackground = new UIView();
            imageBackground.BackgroundColor = Colors.DarkTransparentGray;
			AddSubview(imageBackground);

            nfloat imagePadding = 10;

            nfloat x = image.Frame.X - imagePadding;
            nfloat y = image.Frame.Y - imagePadding;
            nfloat w = image.Frame.Width + 2 * imagePadding;
            nfloat h = w;

			imageBackground.Layer.CornerRadius = w / 2;
            imageBackground.Frame = new CGRect(x, y, w, h);

            BringSubviewToFront(image);
        }

        UITapGestureRecognizer recognizer;

        public void AddGestureRecognizer(Action action)
        {
            recognizer = new UITapGestureRecognizer(action);
            AddGestureRecognizer(recognizer);   
        }

        public void RemoveGestureRecognizer()
        {
            RemoveGestureRecognizer(recognizer);
        }

        public bool IsSet
        {
            get { return Subviews.Any(view => view is MapView); }
        }

        public void SetMap(MapView mapView, MapPos position)
        {
            AddSubview(mapView);
            mapView.Frame = Bounds;

            BringSubviewToFront(image);

            mapView.Zoom = 18;
            mapView.FocusPos = position;

            var layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
            mapView.Layers.Add(layer);

            SetIconRoundWithBackground();
        }

        public void SetPhoto(UIImage image)
        {
            Photo.Image = image;
            SetIconRoundWithBackground();
        }
    }
}
