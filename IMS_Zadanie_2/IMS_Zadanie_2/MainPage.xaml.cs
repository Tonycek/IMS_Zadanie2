using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IMS_Zadanie_2
{
    public partial class MainPage : ContentPage
    {
        HttpClient client;
        private ObservableCollection<Post> _posts;        private const string Url = "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/?q=node/59/";
        public class Post
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }

        public class Test
        {
            public string Test1 { get; set; }
            public string Test2 { get; set; }
            public string Test3 { get; set; }
            public string Test4 { get; set; }
            public string Test5 { get; set; }
            public string Test6 { get; set; }
        }

        public MainPage()
        {
            InitializeComponent();
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            getText();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            
    
        }

        private async Task<string> getText()
        {
            string text = await client.GetStringAsync(Url);
            Int32 indexNodeStart = text.IndexOf("?q=node/") + 12;
            string webStranka = text.Substring(indexNodeStart - 12, 10);
            Int32 indexNodeEnd;

            while (indexNodeStart > 0)
            {
                indexNodeEnd = text.IndexOf("</a>", indexNodeStart);
                string textButton = text.Substring(indexNodeStart, indexNodeEnd - indexNodeStart);
                Button buttonik = new Button();
                buttonik.Text = textButton;//.Substring(0, 11);
                buttonik.StyleId = webStranka;
                buttonik.Clicked += Buttonik_Click;
                stackPanel1.Children.Add(buttonik);

                if (indexNodeStart > text.LastIndexOf("?q=node/"))
                    break;

                indexNodeStart = text.IndexOf("?q=node/", indexNodeStart + 1) + 12;
                webStranka = text.Substring(indexNodeStart - 12, 10);
            }

            return "";
        }
/*
        public async void RefreshDataAsync()
        {
            var response = await client.GetAsync(Url);//GetStringAsync(Url);
         //   var posts = JsonConvert.DeserializeObject<List<Post>>(content);
         //   _posts = new ObservableCollection<Post>(posts);
         //   postsListView.ItemsSource = _posts;

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(content);
                postsListView.ItemsSource = posts;
            }

        }
*/
        private async void Buttonik_Click(object sender, EventArgs e)
        {
            string content = (sender as Button).StyleId.ToString();
            HttpClient webclient1 = new HttpClient();
            string stranka = "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/" + content;
            string text = await client.GetStringAsync(stranka);
            var fs = new FormattedString();       

            int nadpisZaciatok = text.IndexOf("<h3>");
            int nadpisKoniec = text.IndexOf("</h3>");
            if (text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok).Contains("<i class="))
            {
                int icko = text.IndexOf("</i>", nadpisZaciatok + 26) + 4;
                string nadpisok = text.Substring(icko, nadpisKoniec - (icko)) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            else {
                string nadpisok = text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            int spanPosition;
            int pPosition;
            string odstavce = "";
            int indexZaciatok = text.IndexOf("<p");
            indexZaciatok = indexZaciatok + 3;

            string nadpis;
            while (nadpisZaciatok != -1)
            {
                spanPosition = text.IndexOf("<span", nadpisKoniec + 3) + 19;
                pPosition = text.IndexOf("<p", nadpisKoniec + 3);

                if (spanPosition < pPosition)
                {
                    odstavce = text.Substring(spanPosition + 4, text.IndexOf("</span", nadpisKoniec + 4) - spanPosition - 4);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Green, FontSize = 16, FontAttributes = FontAttributes.Italic });

                }

                else if (spanPosition > pPosition)
                {
                    odstavce = text.Substring(pPosition + 3, text.IndexOf("</p>", nadpisKoniec + 4) - pPosition);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Black, FontSize = 12, FontAttributes = FontAttributes.Italic });
                }

                nadpisZaciatok = text.IndexOf("<h3>", nadpisKoniec + 1);
                if (nadpisZaciatok < 0)
                    break;

                nadpisKoniec = text.IndexOf("</h3>", nadpisZaciatok);
                if (text.Substring(nadpisZaciatok, nadpisKoniec - (nadpisZaciatok)).Contains("<i class="))
                {
                    int icko = text.IndexOf("</i>", nadpisZaciatok + 26)+4;
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(icko, nadpisKoniec - icko) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
                else
                {
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(nadpisZaciatok + 4, nadpisKoniec - (nadpisZaciatok+4)) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
            }
            stackPanel1.IsVisible = false;
            stackPanel2.IsVisible = true;
            labelText.FormattedText = fs;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            labelText.Text = "";
           // string content = (sender as Button).StyleId.ToString();
            HttpClient webclient1 = new HttpClient();
            string stranka = "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/?q=node/97/";
            string text = await client.GetStringAsync(stranka);
            var fs = new FormattedString();

            int nadpisZaciatok = text.IndexOf("<h3>");
            int nadpisKoniec = text.IndexOf("</h3>");
            if (text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok).Contains("<i class="))
            {
                int icko = text.IndexOf("</i>", nadpisZaciatok + 26) + 4;
                string nadpisok = text.Substring(icko, nadpisKoniec - (icko)) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            else
            {
                string nadpisok = text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            int spanPosition;
            int pPosition;
            string odstavce = "";
            int indexZaciatok = text.IndexOf("<p");
            indexZaciatok = indexZaciatok + 3;

            string nadpis;
            while (nadpisZaciatok != -1)
            {
                spanPosition = text.IndexOf("<span", nadpisKoniec + 3) + 19;
                pPosition = text.IndexOf("<p", nadpisKoniec + 3);

                if (spanPosition < pPosition)
                {
                    odstavce = text.Substring(spanPosition + 4, text.IndexOf("</span", nadpisKoniec + 4) - spanPosition - 4);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Green, FontSize = 16, FontAttributes = FontAttributes.Italic });

                }

                else if (spanPosition > pPosition)
                {
                    odstavce = text.Substring(pPosition + 3, text.IndexOf("</p>", nadpisKoniec + 4) - pPosition);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Black, FontSize = 12, FontAttributes = FontAttributes.Italic });
                }

                nadpisZaciatok = text.IndexOf("<h3>", nadpisKoniec + 1);
                if (nadpisZaciatok < 0)
                    break;

                nadpisKoniec = text.IndexOf("</h3>", nadpisZaciatok);
                if (text.Substring(nadpisZaciatok, nadpisKoniec - (nadpisZaciatok)).Contains("<i class="))
                {
                    int icko = text.IndexOf("</i>", nadpisZaciatok + 26) + 4;
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(icko, nadpisKoniec - icko) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
                else
                {
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(nadpisZaciatok + 4, nadpisKoniec - (nadpisZaciatok + 4)) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
            }
            stackPanel1.IsVisible = false;
            stackPanel2.IsVisible = true;
            tabulka.IsVisible = true;
            labelText.FormattedText = fs;

            //**************************************************************************************
            tabulka.Intent = TableIntent.Settings;
            StackLayout[] layout = new StackLayout[30]; // = new StackLayout() { Orientation = StackOrientation.Horizontal };
            for (int i = 0; i < 30; i++)
            {
                layout[i] = new StackLayout() { Orientation = StackOrientation.Horizontal };
            }

            int cisloLayout = 0,x = 26;
            var layout1 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout2 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout3 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout4 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout5 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout6 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout7 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout8 = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var layout9 = new StackLayout() { Orientation = StackOrientation.Horizontal };

            int[] indexTabulka = new int[5];
            indexTabulka[0] = text.IndexOf("tabulka-mvi");
            indexTabulka[1] = text.IndexOf("tabulka-mvi", indexTabulka[0] + 1);
            indexTabulka[2] = text.IndexOf("tabulka-mvi", indexTabulka[1] + 1);
            indexTabulka[3] = text.IndexOf("tabulka-mvi", indexTabulka[2] + 1);
            indexTabulka[4] = 100000000;
            int tdZaciatok, tdKoniec;
            int tdKonecnyKoniec = text.LastIndexOf("</td>");
            string[] textyDoGridu = new string[10];
            int poradoveCisloDoGridu = 0;

            for (int i = 0; i < 4; i++)
            {
                int thPositionStart = text.IndexOf("<th>", indexTabulka[i]);
                int thPositionEnd;
                int trKoniec;

                if (i == 0)
                {
                    var data = new Test { Test1 = "1.Ročník", Test2 = "Zimný semester" };
                    layout1.Children.Add(new Label()
                    {
                        Text = "1.Ročník",
                        TextColor = Color.FromHex("#f35e20"),
                        FontSize = 11,
                        VerticalOptions = LayoutOptions.Center
                    });
                    layout1.Children.Add(new Label()
                    {
                        Text = "Zimný semester",
                        TextColor = Color.FromHex("#503026"),
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 11,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    });
                 
                    //tabulka.Root = new TableRoot() {
                    //    new TableSection("Getting Started") {
                    //        new ViewCell() {View = layout}
                    //    }
                    //};
                }

                thPositionEnd = text.IndexOf("</th>", thPositionStart);
                string retazec = text.Substring(thPositionStart + 4, thPositionEnd - (thPositionStart + 4));
                tdZaciatok = text.IndexOf("<td", thPositionEnd) + 4;
                trKoniec = text.IndexOf("</tr>", tdZaciatok);

                while (tdZaciatok < indexTabulka[i + 1])
                {
                    if (tdZaciatok > trKoniec)
                    {
                        trKoniec = text.IndexOf("</tr>", trKoniec + 1);

                        var data = new Test { Test1 = textyDoGridu[0], Test2 = textyDoGridu[1], Test3 = textyDoGridu[2], Test4 = textyDoGridu[3], Test5 = textyDoGridu[4] };
                        //layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                        layout[cisloLayout].Children.Add(new Label()
                        {
                            Text = textyDoGridu[0],
                            TextColor = Color.FromHex("#f35e20"),
                            FontSize = 10,
                            VerticalOptions = LayoutOptions.Center
                        });
                        layout[cisloLayout].Children.Add(new Label()
                        {
                            Text = textyDoGridu[1],
                            TextColor = Color.FromHex("#503026"),
                            VerticalOptions = LayoutOptions.Center,
                            FontSize = 10,
                            HorizontalOptions = LayoutOptions.EndAndExpand
                        });
                        layout[cisloLayout].Children.Add(new Label()
                        {
                            Text = textyDoGridu[2],
                            TextColor = Color.FromHex("#f35e20"),
                            FontSize = 10,
                            VerticalOptions = LayoutOptions.Center
                        });
                        layout[cisloLayout].Children.Add(new Label()
                        {
                            Text = textyDoGridu[3],
                            TextColor = Color.FromHex("#503026"),
                            VerticalOptions = LayoutOptions.Center,
                            FontSize = 10,
                            HorizontalOptions = LayoutOptions.EndAndExpand
                        });
                        layout[cisloLayout].Children.Add(new Label()
                        {
                            Text = textyDoGridu[4],
                            TextColor = Color.FromHex("#f35e20"),
                            FontSize = 10,
                            VerticalOptions = LayoutOptions.Center
                        });

                        //    tabulka.Root = new TableRoot() {
                        //    new TableSection("Getting Started") {
                        //        new ViewCell() {View = layout}
                        //    }
                        //};
                        cisloLayout++;
                        poradoveCisloDoGridu = 0;
                    }
                    tdKoniec = text.IndexOf("</td>", tdZaciatok);
                    string tdText = text.Substring(tdZaciatok, tdKoniec - tdZaciatok);
                    textyDoGridu[poradoveCisloDoGridu] = tdText;

                    if (tdKonecnyKoniec == tdKoniec)
                        break;
                    tdZaciatok = text.IndexOf("<td", tdZaciatok + 1) + 4;
                    poradoveCisloDoGridu++;
                }
                var data1 = new Test { Test1 = textyDoGridu[0], Test2 = textyDoGridu[1], Test3 = textyDoGridu[2], Test4 = textyDoGridu[3], Test5 = textyDoGridu[4] };
               // layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                layout[x].Children.Add(new Label()
                {
                    Text = textyDoGridu[0],
                    TextColor = Color.FromHex("#f35e20"),
                    FontSize = 11,
                    VerticalOptions = LayoutOptions.Center
                });
                layout[x].Children.Add(new Label()
                {
                    Text = textyDoGridu[1],
                    TextColor = Color.FromHex("#503026"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                });
                layout[x].Children.Add(new Label()
                {
                    Text = textyDoGridu[2],
                    TextColor = Color.FromHex("#f35e20"),
                    VerticalOptions = LayoutOptions.Center
                });
                layout[x].Children.Add(new Label()
                {
                    Text = textyDoGridu[3],
                    TextColor = Color.FromHex("#503026"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                });
                layout[x].Children.Add(new Label()
                {
                    Text = textyDoGridu[4],
                    TextColor = Color.FromHex("#f35e20"),
                    FontSize = 11,
                    VerticalOptions = LayoutOptions.Center
                });
                x++;

                //tabulka.Root = new TableRoot() {
                //        new TableSection("Getting Started") {
                //            new ViewCell() {View = layout}
                //        }
                //    };

                if ((i + 1) == 1)
                {
                    var data = new Test { Test1 = "" };
                   // layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    layout2.Children.Add(new Label()
                    {
                        Text = "",
                        TextColor = Color.FromHex("#f35e20"),
                        VerticalOptions = LayoutOptions.Center
                    });
                    //tabulka.Root = new TableRoot() {
                    //    new TableSection("Getting Started") {
                    //        new ViewCell() {View = layout}
                    //    }
                    //};

                    data = new Test { Test1 = "1.Ročník", Test2 = "Letný semester" };

                   // layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    layout3.Children.Add(new Label()
                    {
                        Text = "1.Ročník",
                        TextColor = Color.FromHex("#f35e20"),
                        FontSize = 11,
                        VerticalOptions = LayoutOptions.Center
                    });
                    layout3.Children.Add(new Label()
                    {
                        Text = "Letný semester",
                        TextColor = Color.FromHex("#503026"),
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 11,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    });                  

                    //tabulka.Root = new TableRoot() {
                    //    new TableSection("Getting Started") {
                    //        new ViewCell() {View = layout}
                    //    }
                    //};
                }

                if ((i + 1) == 2)
                {
                    var data = new Test { Test1 = "" };
                  //  layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    layout4.Children.Add(new Label()
                    {
                        Text = "",
                        TextColor = Color.FromHex("#f35e20"),
                        VerticalOptions = LayoutOptions.Center
                    });
                    //tabulka.Root = new TableRoot() {
                    //    new TableSection("Getting Started") {
                    //        new ViewCell() {View = layout}
                    //    }
                    //};

                    data = new Test { Test1 = "2.Ročník", Test2 = "Zimný semester" };
                   // layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    layout5.Children.Add(new Label()
                    {
                        Text = "2.Ročník",
                        TextColor = Color.FromHex("#f35e20"),
                        FontSize = 11,
                        VerticalOptions = LayoutOptions.Center
                    });
                    layout5.Children.Add(new Label()
                    {
                        Text = "Zimný semester",
                        TextColor = Color.FromHex("#503026"),
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 11,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    });

                    //tabulka.Root = new TableRoot() {
                    //    new TableSection("Getting Started") {
                    //        new ViewCell() {View = layout}
                    //    }
                    //};
                }

                if ((i + 1) == 3)
                {
                    var data = new Test { Test1 = "" };
                   // layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    layout6.Children.Add(new Label()
                    {
                        Text = "",
                        TextColor = Color.FromHex("#f35e20"),
                        VerticalOptions = LayoutOptions.Center
                    });
                    //tabulka.Root = new TableRoot() {
                    //    new TableSection("Getting Started") {
                    //        new ViewCell() {View = layout}
                    //    }
                    //};

                    data = new Test { Test1 = "2.Ročník", Test2 = "Letný semester" };
                   // layout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    layout7.Children.Add(new Label()
                    {
                        Text = "2.Ročník",
                        TextColor = Color.FromHex("#f35e20"),
                        FontSize = 11,
                        VerticalOptions = LayoutOptions.Center
                    });
                    layout7.Children.Add(new Label()
                    {
                        Text = "Letný semester",
                        TextColor = Color.FromHex("#503026"),
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 11,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    });

                    //tabulka.Root = new TableRoot() {
                    //    new TableSection("Getting Started") {
                    //        new ViewCell() {View = layout}
                    //    }
                    //};
                }

                //tabulka.Root = new TableRoot() {
                //        new TableSection("Getting Started") {
                //            new ViewCell() {View = layout1},
                //            new ViewCell() {View = layout2},
                //            new ViewCell() {View = layout3},
                //            new ViewCell() {View = layout4},
                //            new ViewCell() {View = layout5},
                //            new ViewCell() {View = layout6},
                //            new ViewCell() {View = layout7}
                //        }
                //    };
                //textyDoGridu = new string[10];
                poradoveCisloDoGridu = 0;

                thPositionStart = text.IndexOf("<th>", thPositionStart + 5);

            }

            tabulka.Root = new TableRoot() {
                    new TableSection("Studijny program") {
                        new ViewCell() {View = layout1},
                        new ViewCell() {View = layout[0]},
                        new ViewCell() {View = layout[1]},
                        new ViewCell() {View = layout[2]},
                        new ViewCell() {View = layout[3]},
                        new ViewCell() {View = layout[4]},
                        new ViewCell() {View = layout[5]},
                        new ViewCell() {View = layout[26]},
                        new ViewCell() {View = layout2},

                        new ViewCell() {View = layout3},
                        new ViewCell() {View = layout[6]},
                        new ViewCell() {View = layout[7]},
                        new ViewCell() {View = layout[8]},
                        new ViewCell() {View = layout[9]},
                        new ViewCell() {View = layout[10]},
                        new ViewCell() {View = layout[11]},
                        new ViewCell() {View = layout[27]},
                        new ViewCell() {View = layout4},

                        
                        new ViewCell() {View = layout5},
                        new ViewCell() {View = layout[12]},
                        new ViewCell() {View = layout[13]},
                        new ViewCell() {View = layout[14]},
                        new ViewCell() {View = layout[15]},
                        new ViewCell() {View = layout[16]},
                        new ViewCell() {View = layout[17]},
                        new ViewCell() {View = layout[18]},
                        new ViewCell() {View = layout[19]},
                        new ViewCell() {View = layout[28]},
                        new ViewCell() {View = layout6},


                        new ViewCell() {View = layout7},
                        new ViewCell() {View = layout[20]},
                        new ViewCell() {View = layout[22]},
                        new ViewCell() {View = layout[29]}
                    }
                };
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            stackPanel1.IsVisible = false;
            stackPanel2.IsVisible = true;
            tabulka.IsVisible = false;
            Device.BeginInvokeOnMainThread(async () => {

                RestClient client = new RestClient();
                var wingetresult = await client.Get<WingetResult>("http://validate.jsontest.com/?json=%7B%22key%22:%22value%22");
                if (wingetresult != null)
                {
                    labelText.Text = wingetresult.error_info;
                }
            });
        }
    }
}

//new ViewCell() { View = layout},
//                            new ViewCell() { View = layout2}
