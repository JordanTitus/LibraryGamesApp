using LibraryGamesApp.Utilities_Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LibraryGamesApp.UserControls_Folder
{
    /// <summary>
    /// User control for managing a book replacement game.
    /// </summary>
    public partial class BookReplacingControl : UserControl
    {
        private double spacing = 10;
        private CallNumberGenerator callNumber = new CallNumberGenerator();
        private Random random = new Random();
        private DispatcherTimer timer;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private List<string> bookList = new List<string>();
        private List<Rectangle> rectanglesList = new List<Rectangle>();
        private Point startPoint;
        private bool isDragging = false;
        private double originalLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookReplacingControl"/> class.
        /// </summary>
        public BookReplacingControl()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            Panel.SetZIndex(timerLabel, 999);
        }

        private void CreateBooks()
        {
            double x = 10;

            for (int i = 0; i < 10; i++)
            {
                bookList.Add(callNumber.GenCallNumbers());
            }

            foreach (string item in bookList)
            {
                SolidColorBrush brushCol = GetRandomBrush();

                Rectangle rectangle = new Rectangle
                {
                    Width = 60,
                    Height = 398,
                    Fill = brushCol,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Tag = item
                };
                rectangle.IsHitTestVisible = true;

                TextBlock txtBlock = new TextBlock
                {
                    Text = item.ToString(),
                    Tag = item,
                    Width = 60,
                    Foreground = Brushes.Black,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Canvas.SetLeft(rectangle, x);
                Canvas.SetTop(rectangle, 1);
                CanvasBook.Children.Add(rectangle);

                Canvas.SetLeft(txtBlock, x);
                Canvas.SetBottom(txtBlock, 50);
                CanvasBook.Children.Add(txtBlock);

                rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
                rectangle.MouseMove += Rectangle_MouseMove;
                rectangle.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;

                rectanglesList.Add(rectangle);
                x += 60 + spacing;
            }
        }

        public SolidColorBrush GetRandomBrush()
        {
            byte red = (byte)random.Next(256);
            byte blue = (byte)random.Next(256);
            byte green = (byte)random.Next(256);

            SolidColorBrush randomBrush = new SolidColorBrush(Color.FromRgb(red, green, blue));
            return randomBrush;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            isDragging = true;

            Rectangle rectangle = sender as Rectangle;
            originalLeft = Canvas.GetLeft(rectangle);

            startPoint = e.GetPosition(null);
            rectangle.CaptureMouse();
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Rectangle movingRectangle = sender as Rectangle;
                Point currentPosition = e.GetPosition(null);

                double deltaX = currentPosition.X - startPoint.X;
                double deltaY = currentPosition.Y - startPoint.Y;

                double newLeft = Canvas.GetLeft(movingRectangle) + deltaX;
                double newTop = Canvas.GetTop(movingRectangle) + deltaY;

                newLeft = Math.Max(0, Math.Min(CanvasBook.ActualWidth - movingRectangle.ActualWidth, newLeft));
                newTop = Math.Max(0, Math.Min(CanvasBook.ActualHeight - movingRectangle.ActualHeight, newTop));

                foreach (UIElement element in CanvasBook.Children)
                {
                    if (element is TextBlock txtBlock && txtBlock.Tag == movingRectangle.Tag)
                    {
                        Canvas.SetLeft(txtBlock, newLeft);
                        Canvas.SetTop(txtBlock, newTop);
                    }
                }

                Canvas.SetLeft(movingRectangle, newLeft);
                Canvas.SetTop(movingRectangle, newTop);

                startPoint = currentPosition;
            }
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            Rectangle movingRectangle = sender as Rectangle;
            movingRectangle.ReleaseMouseCapture();

            double newLeft = Canvas.GetLeft(movingRectangle);
            double newTop = Canvas.GetTop(movingRectangle);

            bool collisionDetected = false;

            foreach (Rectangle otherRectangle in rectanglesList)
            {
                if (otherRectangle != movingRectangle && CollisionTrue(movingRectangle, otherRectangle, newLeft, newTop))
                {
                    double otherLeft = Canvas.GetLeft(otherRectangle);
                    double otherTop = Canvas.GetTop(otherRectangle);

                    Canvas.SetLeft(otherRectangle, originalLeft);
                    Canvas.SetTop(otherRectangle, Canvas.GetTop(movingRectangle));

                    Canvas.SetLeft(movingRectangle, otherLeft);
                    Canvas.SetTop(movingRectangle, otherTop);

                    collisionDetected = true;
                    break;
                }
            }

            if (!collisionDetected)
            {
                UpdateBookList();
            }
        }

        private void UpdateBookList()
        {
            rectanglesList = rectanglesList.OrderBy(Rectangle => Canvas.GetLeft(Rectangle)).ToList();

            bookList.Clear();

            foreach (Rectangle rect in rectanglesList)
            {
                string callNumber = (string)rect.Tag;
                bookList.Add(callNumber);
            }

            CheckCompletion();
        }

        private bool CollisionTrue(Rectangle rect1, Rectangle rect2, double newLeft, double newTop)
        {
            Rect rect1Bounds = new Rect(newLeft, newTop, rect1.Width, rect1.Height);

            double rect2Left = Canvas.GetLeft(rect2);
            double rect2Top = Canvas.GetTop(rect2);
            Rect rect2Bounds = new Rect(rect2Left, rect2Top, rect2.Width, rect2.Height);

            return rect1Bounds.IntersectsWith(rect2Bounds);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            UpdateTimerLabel();
        }

        private void UpdateTimerLabel()
        {
            timerLabel.Content = $"Time:{elapsedTime:mm\\:ss}";
        }

        private bool CheckCompletion()
        {
            bool success = false;

            for (int i = 0; i < bookList.Count; i++)
            {
                success = true;
            }

            return success;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateBooks();
            timer.Start();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            UpdateTimerLabel();

            CheckCompletion();
        }
    }
}
