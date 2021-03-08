using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpMakes.Helpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaSharpMakes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ElipticCircle : ContentPage
    {
        public ElipticCircle()
        {
            InitializeComponent();
            eliptic_circle_iterator = 8;
            explodeOffset = 50;
            ball_size = 30;
            _rotatingSpeed = 10;
            _degrees = 0;
        }

        private int eliptic_circle_iterator { get; set; }
        private float explodeOffset { get; set; }
        private float ball_size { get; set; }
        private bool _shouldRotate;
        private float _degrees;
        private double _rotatingSpeed { get; set; }
        private bool _reverseSpinning { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _shouldRotate = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _shouldRotate = false;
        }
        async Task AnimationLoop(int extension = 0)
        {
            while (_shouldRotate)
            {
                skiaView.InvalidateSurface();
                await Task.Delay(TimeSpan.FromMilliseconds(_rotatingSpeed)); //fastest is 10 millisecond. 
            }
        }
        private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);
            float radius = Math.Min(info.Width / 2, info.Height / 2) - 2 * explodeOffset;

            //draw the eliptic circle
            using (SKPaint fillMarkDotCirclePaint = new SKPaint())
            {
                var eliptic_circle_test_angle = 360 / eliptic_circle_iterator / 2 + _degrees;
                if (_shouldRotate)
                {
                    eliptic_circle_test_angle += _degrees;
                }

                fillMarkDotCirclePaint.Style = SKPaintStyle.Fill;
                for (int ii = 0; ii < eliptic_circle_iterator; ii++)
                {
                    fillMarkDotCirclePaint.Color = Color.FromHex(General.Colors[ii]).ToSKColor();
                    //calculate the  point on the circle

                    General.PointOnCircle(radius, eliptic_circle_test_angle, center.X, center.Y, out float circle_x, out float circle_y);

                    surface.Canvas.DrawCircle(circle_x, circle_y, ball_size, fillMarkDotCirclePaint);

                    eliptic_circle_test_angle += 360 / eliptic_circle_iterator;
                    if (eliptic_circle_test_angle > 360)
                        eliptic_circle_test_angle -= 360;
                }
            }

            if (_shouldRotate)
                IncrementDegrees();
        }

        private void IncrementDegrees()
        {
            if (_reverseSpinning)
            {
                if (_degrees >= 360)
                {
                    _degrees = _degrees - 360;
                }
                _degrees -= 3.6f;
            }
            else
            {
                if (_degrees >= 360)
                {
                    _degrees = _degrees - 360;
                }
                _degrees += 3.6f;
            }
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            eliptic_circle_iterator = int.Parse(((Stepper)sender).Value.ToString());
            numberLabel.Text = $"Number: {eliptic_circle_iterator}";
            skiaView.InvalidateSurface();
        }

        private void Stepper_Offset_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            explodeOffset = float.Parse(((Stepper)sender).Value.ToString());
            skiaView.InvalidateSurface();
        }

        private void Stepper_Ball_Size_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ball_size = float.Parse(((Stepper)sender).Value.ToString());
            skiaView.InvalidateSurface();
        }

        private void rotateSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            _shouldRotate = e.Value;
            if (e.Value)
            {
                AnimationLoop();
            }
        }

        private void Stepper_Rotation_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _rotatingSpeed = 100 - (double.Parse(((Stepper)sender).Value.ToString()) * 10);
            speedLabel.Text = $"Speed: {((Stepper)sender).Value}";
        }

        private void directionSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            _reverseSpinning = e.Value;
        }
    }
}