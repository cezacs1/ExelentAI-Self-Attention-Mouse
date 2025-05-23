using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

// Token: 0x0200000E RID: 14
public class 469F9E21 : Form
{
	// Token: 0x06000049 RID: 73 RVA: 0x00002FE4 File Offset: 0x000013E4
	public 469F9E21()
	{
		this.D7AD5897();
		base.ClientSize = new Size(800, 600);
		this.Text = "Mouse Predictor (Transformer) – .NET 4.8";
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		this.7824F31F = this.E68C7833();
		Timer timer = new Timer
		{
			Interval = 10
		};
		timer.Tick += this.9A0F63BC;
		timer.Start();
		base.Paint += this.752EC8AB;
		base.KeyDown += this.901D9104;
		base.Load += this.F8B1A908;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003114 File Offset: 0x00001514
	private void F8B1A908(object C62283A6, EventArgs 8B98A5BA)
	{
		bool flag = !469F9E21.DAA37E9E && File.Exists("mouse_transformer_model_v3.json");
		if (flag)
		{
			Console.WriteLine("Test modu, mouse_transformer_model_v3.json yükleniyor...");
			this.34305001.AC8AA79E("mouse_transformer_model_v3.json");
		}
		else
		{
			bool daa37E9E = 469F9E21.DAA37E9E;
			if (daa37E9E)
			{
				Console.WriteLine("Eğitim modu, yeni model oluşturulacak.");
			}
			else
			{
				Console.WriteLine("Test modu, mouse_transformer_model_v3.json bulunamadı. Varsayılan ağ.");
			}
		}
		this.A59D2B82(469F9E21.DAA37E9E ? "Eğitim (Transformer). Fareyi hedeflere sürükle! (S: Eğit)" : "Test (Transformer). V: Otomatik.");
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003195 File Offset: 0x00001595
	private PointF E68C7833()
	{
		return new PointF((float)this.FC366037.Next(15, 785), (float)this.FC366037.Next(15, 585));
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000031C2 File Offset: 0x000015C2
	private void A59D2B82(string F1B3E4B9)
	{
		MessageBox.Show(this, F1B3E4B9, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000031D8 File Offset: 0x000015D8
	private double 0B8E7436(PointF F083D91A, PointF D5BC5734)
	{
		double num = (double)(F083D91A.X - D5BC5734.X);
		double num2 = (double)(F083D91A.Y - D5BC5734.Y);
		return Math.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003218 File Offset: 0x00001618
	private double[] FC3F15A1(PointF 73A31DA7, PointF 70097E20, PointF 5225C093)
	{
		double[] array = new double[5];
		array[0] = (double)((5225C093.X - 73A31DA7.X) / 800f);
		array[1] = (double)((5225C093.Y - 73A31DA7.Y) / 600f);
		double[] array2 = new double[]
		{
			(double)(73A31DA7.X - 70097E20.X),
			(double)(73A31DA7.Y - 70097E20.Y)
		};
		double num = Math.Sqrt(array2[0] * array2[0] + array2[1] * array2[1]);
		array[2] = ((num > 0.001) ? (array2[0] / num) : 0.0);
		array[3] = ((num > 0.001) ? (array2[1] / num) : 0.0);
		array[4] = Math.Min(1.0, num / 12.0);
		return array;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003300 File Offset: 0x00001700
	private double[] ECA6D238(PointF BA1E293E, PointF A2869E0B, PointF 4C3A1D9B)
	{
		List<double[]> list = new List<double[]>();
		List<PointF> list2 = new List<PointF>();
		list2.Add(BA1E293E);
		bool flag = !BA1E293E.Equals(A2869E0B);
		if (flag)
		{
			list2.Add(A2869E0B);
		}
		else
		{
			list2.Add(BA1E293E);
		}
		PointF pointF = (list2.Count > 1) ? list2.Last<PointF>() : BA1E293E;
		foreach (PointF pointF2 in this.7A09D303.ToArray().Reverse<PointF>())
		{
			bool flag2 = list2.Count >= 5;
			if (flag2)
			{
				break;
			}
			bool flag3 = !pointF2.Equals(pointF);
			if (flag3)
			{
				list2.Add(pointF2);
				pointF = pointF2;
			}
		}
		while (list2.Count < 5 && list2.Count > 0)
		{
			list2.Add(list2.Last<PointF>());
		}
		while (list2.Count < 5)
		{
			list2.Add(BA1E293E);
		}
		for (int i = 0; i < 5; i++)
		{
			PointF 73A31DA = list2[i];
			PointF 70097E = (i + 1 < list2.Count) ? list2[i + 1] : list2[i];
			list.Add(this.FC3F15A1(73A31DA, 70097E, 4C3A1D9B));
		}
		List<double> list3 = new List<double>();
		foreach (double[] collection in list)
		{
			list3.AddRange(collection);
		}
		bool flag4 = list3.Count != 25;
		double[] result;
		if (flag4)
		{
			Console.WriteLine(string.Format("UYARI: PrepareFeatureVectorForTransformer - Boyut hatası. Gelen: {0}, Beklenen: {1}", list3.Count, 25));
			double[] array = new double[25];
			int num = 0;
			while (num < array.Length && num < list3.Count)
			{
				array[num] = list3[num];
				num++;
			}
			result = array;
		}
		else
		{
			result = list3.ToArray();
		}
		return result;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00003558 File Offset: 0x00001958
	private void 03051D0C()
	{
		Point point = base.PointToClient(Cursor.Position);
		bool flag = this.7A09D303.Count >= 100;
		if (flag)
		{
			this.7A09D303.Dequeue();
		}
		bool flag2 = !this.7A09D303.Any<PointF>() || !this.7A09D303.Last<PointF>().Equals(point);
		if (flag2)
		{
			this.7A09D303.Enqueue(point);
		}
		bool flag3 = this.ED146FA3 && this.F70BDBB8 != null && this.7C0529A2 != null;
		PointF pointF;
		PointF pointF2;
		if (flag3)
		{
			pointF = this.F70BDBB8.Value;
			pointF2 = this.7C0529A2.Value;
		}
		else
		{
			pointF = point;
			bool flag4 = this.7A09D303.Count > 1;
			if (flag4)
			{
				pointF2 = (this.7A09D303.Last<PointF>().Equals(point) ? this.7A09D303.ElementAt(this.7A09D303.Count - 2) : this.7A09D303.Last<PointF>());
			}
			else
			{
				bool flag5 = this.7A09D303.Count == 1;
				if (flag5)
				{
					pointF2 = this.7A09D303.Last<PointF>();
				}
				else
				{
					pointF2 = point;
				}
			}
		}
		bool flag6 = this.0B8E7436(pointF, this.7824F31F) < 15.0;
		if (flag6)
		{
			this.7824F31F = this.E68C7833();
		}
		PointF pointF3 = 469F9E21.DAA37E9E ? point : pointF;
		PointF pointF4 = pointF2;
		bool daa37E9E = 469F9E21.DAA37E9E;
		if (daa37E9E)
		{
			bool flag7 = this.7A09D303.Count > 1;
			if (flag7)
			{
				bool flag8 = this.7A09D303.Last<PointF>().Equals(pointF3);
				if (flag8)
				{
					pointF4 = this.7A09D303.ElementAt(this.7A09D303.Count - 2);
				}
				else
				{
					pointF4 = this.7A09D303.Last<PointF>();
				}
			}
			else
			{
				pointF4 = pointF3;
			}
			this.A0BC03A8(pointF3, pointF4);
		}
		else
		{
			bool ed146FA = this.ED146FA3;
			if (ed146FA)
			{
				PointF item = this.D02CC822(pointF3, pointF4).Item1;
				Cursor.Position = base.PointToScreen(Point.Round(item));
				this.7C0529A2 = new PointF?(pointF3);
				this.F70BDBB8 = new PointF?(item);
			}
			else
			{
				this.F70BDBB8 = null;
				this.7C0529A2 = null;
			}
		}
		this.C995C122++;
		bool flag9 = this.44352091.ElapsedMilliseconds >= 1000L;
		if (flag9)
		{
			this.Text = string.Format("Mouse Predictor (Transformer) – FPS: {0}", this.C995C122);
			this.C995C122 = 0;
			this.44352091.Restart();
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00003844 File Offset: 0x00001C44
	private void A0BC03A8(PointF 0522389D, PointF 9C31022E)
	{
		double num = Math.Sqrt(Math.Pow((double)(0522389D.X - 9C31022E.X), 2.0) + Math.Pow((double)(0522389D.Y - 9C31022E.Y), 2.0));
		bool flag = num <= 0.01 && this.0626943F.Count > 0;
		if (!flag)
		{
			double[] item = this.ECA6D238(0522389D, 9C31022E, this.7824F31F);
			double[] array = new double[]
			{
				(double)(this.7824F31F.X - 0522389D.X),
				(double)(this.7824F31F.Y - 0522389D.Y)
			};
			double num2 = Math.Sqrt(array[0] * array[0] + array[1] * array[1]);
			double num3 = (num2 > 0.001) ? (array[0] / num2) : 0.0;
			double num4 = (num2 > 0.001) ? (array[1] / num2) : 0.0;
			double[] array2 = new double[]
			{
				(double)(0522389D.X - 9C31022E.X),
				(double)(0522389D.Y - 9C31022E.Y)
			};
			double num5 = Math.Sqrt(array2[0] * array2[0] + array2[1] * array2[1]);
			double num6 = num5 / 4.0;
			double num7 = 2.8;
			double num8 = -1.0;
			bool flag2 = num7 > 0.001;
			if (flag2)
			{
				double num9 = (num6 - 0.2) / num7;
				num8 = Math.Max(-1.0, Math.Min(1.0, num9 * 2.0 - 1.0));
			}
			else
			{
				bool flag3 = num6 >= 3.0;
				if (flag3)
				{
					num8 = 1.0;
				}
			}
			this.0626943F.Add(new ValueTuple<double[], double[]>(item, new double[]
			{
				num3,
				num4,
				num8
			}));
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00003A64 File Offset: 0x00001E64
	[return: TupleElementNames(new string[]
	{
		"NextPosition",
		"SpeedMagnitude"
	})]
	private ValueTuple<PointF, double> D02CC822(PointF E20A65A5, PointF C52B6CB4)
	{
		double[] 500BAA = this.ECA6D238(E20A65A5, C52B6CB4, this.7824F31F);
		double[] array = this.34305001.E1B6032E(500BAA, false);
		double num = array[0];
		double num2 = array[1];
		double num3 = array[2];
		double num4 = Math.Sqrt(num * num + num2 * num2);
		double num5 = (num4 > 0.001) ? (num / num4) : 0.0;
		double num6 = (num4 > 0.001) ? (num2 / num4) : 0.0;
		double num7 = (num3 + 1.0) / 2.0;
		double num8 = 0.2 + num7 * 2.8;
		double num9 = num8 * 4.0;
		PointF pointF = new PointF((float)((double)E20A65A5.X + num5 * num9), (float)((double)E20A65A5.Y + num6 * num9));
		return new ValueTuple<PointF, double>(new PointF(Math.Max(0f, Math.Min(800f, pointF.X)), Math.Max(0f, Math.Min(600f, pointF.Y))), num9);
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00003B94 File Offset: 0x00001F94
	[return: TupleElementNames(new string[]
	{
		"Point",
		"SpeedForNextSegment"
	})]
	private List<ValueTuple<PointF, double>> AE37D3B7(PointF 1B8EF89C, PointF A8B6991C, int 39B83A91 = 50)
	{
		List<ValueTuple<PointF, double>> list = new List<ValueTuple<PointF, double>>();
		PointF pointF = 1B8EF89C;
		PointF pointF2 = A8B6991C;
		Queue<PointF> queue = new Queue<PointF>(this.7A09D303);
		bool flag = !queue.Contains(pointF2);
		if (flag)
		{
			queue.Enqueue(pointF2);
		}
		bool flag2 = !queue.Contains(pointF);
		if (flag2)
		{
			queue.Enqueue(pointF);
		}
		while (queue.Count > 100)
		{
			queue.Dequeue();
		}
		for (int i = 0; i < 39B83A91; i++)
		{
			ValueTuple<PointF, double> valueTuple = this.D02CC822(pointF, pointF2);
			PointF item = valueTuple.Item1;
			double item2 = valueTuple.Item2;
			list.Add(new ValueTuple<PointF, double>(pointF, item2));
			bool flag3 = this.0B8E7436(item, this.7824F31F) < 15.0 || (i > 5 && this.0B8E7436(pointF, item) < 0.1);
			if (flag3)
			{
				list.Add(new ValueTuple<PointF, double>(item, 0.0));
				break;
			}
			pointF2 = pointF;
			pointF = item;
		}
		return list;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003CAC File Offset: 0x000020AC
	private void 752EC8AB(object 1324348E, PaintEventArgs C311DE24)
	{
		Graphics graphics = C311DE24.Graphics;
		graphics.Clear(this.BackColor);
		using (SolidBrush solidBrush = new SolidBrush(Color.Tomato))
		{
			graphics.FillEllipse(solidBrush, this.7824F31F.X - 15f, this.7824F31F.Y - 15f, 30f, 30f);
		}
		PointF pointF = base.PointToClient(Cursor.Position);
		using (SolidBrush solidBrush2 = new SolidBrush(Color.SkyBlue))
		{
			graphics.FillEllipse(solidBrush2, pointF.X - 7f, pointF.Y - 7f, 14f, 14f);
		}
		bool flag = this.ED146FA3 && this.F70BDBB8 != null;
		if (flag)
		{
			using (SolidBrush solidBrush3 = new SolidBrush(Color.LimeGreen))
			{
				graphics.FillEllipse(solidBrush3, this.F70BDBB8.Value.X - 5f, this.F70BDBB8.Value.Y - 5f, 10f, 10f);
			}
			List<ValueTuple<PointF, double>> list = this.AE37D3B7(this.F70BDBB8.Value, this.7C0529A2 ?? this.F70BDBB8.Value, 30);
			bool flag2 = list.Count > 1;
			if (flag2)
			{
				using (Pen pen = new Pen(Color.FromArgb(100, Color.LightGreen), 2f))
				{
					graphics.DrawLines(pen, list.Select(new Func<ValueTuple<PointF, double>, PointF>(469F9E21.F38157B7.<>9.E4AE7D14)).ToArray<PointF>());
				}
			}
		}
		bool flag3 = this.6683C40A != null && this.6683C40A.Count > 1;
		if (flag3)
		{
			using (Pen pen2 = new Pen(Color.FromArgb(150, Color.OrangeRed), 2f))
			{
				graphics.DrawLines(pen2, this.6683C40A.Select(new Func<ValueTuple<PointF, double>, PointF>(469F9E21.F38157B7.<>9.3B13BD86)).ToArray<PointF>());
			}
		}
		bool flag4 = 469F9E21.DAA37E9E && this.7A09D303.Count > 1;
		if (flag4)
		{
			using (Pen pen3 = new Pen(Color.FromArgb(100, Color.Green), 2f))
			{
				graphics.DrawLines(pen3, this.7A09D303.ToArray());
			}
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003FC0 File Offset: 0x000023C0
	private void 901D9104(object 6E89DCB7, KeyEventArgs 329572AF)
	{
		bool flag = 329572AF.KeyCode == Keys.V && !469F9E21.DAA37E9E;
		if (flag)
		{
			this.ED146FA3 = !this.ED146FA3;
			this.6683C40A = null;
			bool flag2 = !this.ED146FA3;
			if (flag2)
			{
				this.F70BDBB8 = null;
				this.7C0529A2 = null;
			}
			else
			{
				this.F70BDBB8 = new PointF?(base.PointToClient(Cursor.Position));
				this.7C0529A2 = new PointF?((this.7A09D303.Count > 0) ? this.7A09D303.Last<PointF>() : this.F70BDBB8.Value);
				bool flag3 = this.7A09D303.Count > 1 && this.7C0529A2.Value.Equals(this.F70BDBB8.Value);
				if (flag3)
				{
					this.7C0529A2 = new PointF?(this.7A09D303.ElementAt(this.7A09D303.Count - 2));
				}
			}
			base.Invalidate();
		}
		bool flag4 = 329572AF.KeyCode == Keys.S && 469F9E21.DAA37E9E && !this.BF1C881B;
		if (flag4)
		{
			this.BF1C881B = true;
			Task.Run(new Action(this.ED2E7FAC));
		}
		bool flag5 = 329572AF.KeyCode == Keys.P && !469F9E21.DAA37E9E && !this.ED146FA3;
		if (flag5)
		{
			PointF pointF = base.PointToClient(Cursor.Position);
			PointF a8B6991C = (this.7A09D303.Count > 0) ? this.7A09D303.Last<PointF>() : pointF;
			bool flag6 = this.7A09D303.Count > 1 && this.7A09D303.Last<PointF>().Equals(pointF);
			if (flag6)
			{
				a8B6991C = this.7A09D303.ElementAt(this.7A09D303.Count - 2);
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			this.6683C40A = this.AE37D3B7(pointF, a8B6991C, 100);
			stopwatch.Stop();
			Console.WriteLine(string.Format("P-Path: {0:F2} ms, {1} pts.", stopwatch.Elapsed.TotalMilliseconds, this.6683C40A.Count));
			base.Invalidate();
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0000422C File Offset: 0x0000262C
	private void 9696752F()
	{
		469F9E21.861A9526 861A = new 469F9E21.861A9526();
		861A.B69D0500 = this;
		Console.WriteLine(string.Format("RunTraining çağrıldı. trainingData.Count: {0}", this.0626943F.Count));
		bool flag = this.0626943F.Count > 0;
		if (flag)
		{
			string[] array = new string[5];
			array[0] = "İlk trainingData örneği - feats: [";
			array[1] = string.Join(", ", this.0626943F[0].Item1.Select(new Func<double, string>(469F9E21.F38157B7.<>9.E9B20113)));
			array[2] = "], tgt: [";
			array[3] = string.Join(", ", this.0626943F[0].Item2.Select(new Func<double, string>(469F9E21.F38157B7.<>9.01119A15)));
			array[4] = "]";
			Console.WriteLine(string.Concat(array));
		}
		861A.2A8F7825 = this.0626943F.Select(new Func<ValueTuple<double[], double[]>, double[]>(469F9E21.F38157B7.<>9.C3813935)).ToArray<double[]>();
		double[][] array2 = this.0626943F.Select(new Func<ValueTuple<double[], double[]>, double[]>(469F9E21.F38157B7.<>9.2FB72616)).ToArray<double[]>();
		Console.WriteLine("X.Length: " + 861A.2A8F7825.Length.ToString());
		Console.WriteLine("Y.Length: " + array2.Length.ToString());
		bool flag2 = 861A.2A8F7825.Length < 50;
		if (flag2)
		{
			base.BeginInvoke(new Action(861A.772EDB05));
			this.BF1C881B = false;
		}
		else
		{
			base.BeginInvoke(new Action(861A.B9B9DC85));
			861A.1BAFC418 = 20;
			double 3CB43FAF = 5E-05;
			this.34305001.672ADB2D(861A.2A8F7825, array2, 861A.1BAFC418, 3CB43FAF, new Action<int, double>(861A.E891D3BD));
			this.34305001.FFA3B102("mouse_transformer_model_v3.json");
			base.BeginInvoke(new Action(861A.798A180F));
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0000446C File Offset: 0x0000286C
	protected override void Dispose(bool D92EA931)
	{
		bool flag = D92EA931 && this.1788108C != null;
		if (flag)
		{
			this.1788108C.Dispose();
		}
		base.Dispose(D92EA931);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000044A4 File Offset: 0x000028A4
	private void D7AD5897()
	{
		base.SuspendLayout();
		base.AutoScaleDimensions = new SizeF(6f, 13f);
		base.AutoScaleMode = AutoScaleMode.Font;
		base.ClientSize = new Size(800, 450);
		base.Name = "PredictorForm";
		this.Text = "Form1";
		base.Load += this.F8B1A908;
		base.ResumeLayout(false);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00004528 File Offset: 0x00002928
	[CompilerGenerated]
	private void 9A0F63BC(object EB0C6435, EventArgs 9F3D78AB)
	{
		this.03051D0C();
		base.Invalidate();
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00004539 File Offset: 0x00002939
	[CompilerGenerated]
	private void ED2E7FAC()
	{
		this.9696752F();
	}

	// Token: 0x04000021 RID: 33
	private const int B5953512 = 800;

	// Token: 0x04000022 RID: 34
	private const int 8D066E23 = 600;

	// Token: 0x04000023 RID: 35
	private const int B50C7235 = 100;

	// Token: 0x04000024 RID: 36
	private const int BCBFEC99 = 15;

	// Token: 0x04000025 RID: 37
	public static bool DAA37E9E = true;

	// Token: 0x04000026 RID: 38
	private const int B7A83AA9 = 100;

	// Token: 0x04000027 RID: 39
	private const double 1B3C810E = 4.0;

	// Token: 0x04000028 RID: 40
	private const double 66AC4226 = 12.0;

	// Token: 0x04000029 RID: 41
	private const double 78B1C59B = 0.2;

	// Token: 0x0400002A RID: 42
	private const double 2D2F4404 = 3.0;

	// Token: 0x0400002B RID: 43
	private const int 45ABA99D = 5;

	// Token: 0x0400002C RID: 44
	private const int 3BA01401 = 5;

	// Token: 0x0400002D RID: 45
	private const int 04AFA3A3 = 32;

	// Token: 0x0400002E RID: 46
	private const int 81906E87 = 4;

	// Token: 0x0400002F RID: 47
	private const int ED826EB6 = 2;

	// Token: 0x04000030 RID: 48
	private const int 3EB8778A = 64;

	// Token: 0x04000031 RID: 49
	private const double 0C9753B8 = 0.1;

	// Token: 0x04000032 RID: 50
	private readonly 469F9E21.1621B7AD 34305001 = new 469F9E21.1621B7AD();

	// Token: 0x04000033 RID: 51
	[TupleElementNames(new string[]
	{
		"feats",
		"tgt"
	})]
	private readonly List<ValueTuple<double[], double[]>> 0626943F = new List<ValueTuple<double[], double[]>>();

	// Token: 0x04000034 RID: 52
	private readonly Queue<PointF> 7A09D303 = new Queue<PointF>(100);

	// Token: 0x04000035 RID: 53
	private PointF 7824F31F;

	// Token: 0x04000036 RID: 54
	private bool ED146FA3 = false;

	// Token: 0x04000037 RID: 55
	private readonly Random FC366037 = new Random();

	// Token: 0x04000038 RID: 56
	private readonly Stopwatch 44352091 = Stopwatch.StartNew();

	// Token: 0x04000039 RID: 57
	private int C995C122 = 0;

	// Token: 0x0400003A RID: 58
	private bool BF1C881B = false;

	// Token: 0x0400003B RID: 59
	private PointF? F70BDBB8 = null;

	// Token: 0x0400003C RID: 60
	private PointF? 7C0529A2 = null;

	// Token: 0x0400003D RID: 61
	private const string 5D2F5FBA = "mouse_transformer_model_v3.json";

	// Token: 0x0400003E RID: 62
	[TupleElementNames(new string[]
	{
		"Point",
		"SpeedForNextSegment"
	})]
	private List<ValueTuple<PointF, double>> 6683C40A = null;

	// Token: 0x0400003F RID: 63
	private IContainer 1788108C = null;

	// Token: 0x0200000F RID: 15
	public abstract class FD81572E
	{
		// Token: 0x0600005C RID: 92
		public abstract double[] 7B132382(double[] EABF1585, bool 7C3CD9A8 = false);

		// Token: 0x0600005D RID: 93
		public abstract double[] 721B1D97(double[] 1B3C1D06, double 7DA015B6);

		// Token: 0x0600005E RID: 94
		public abstract object 3A811634();

		// Token: 0x0600005F RID: 95
		public abstract void 4486B5B7(JsonElement 468198A3);
	}

	// Token: 0x02000010 RID: 16
	public static class C3259C27
	{
		// Token: 0x06000061 RID: 97 RVA: 0x0000454C File Offset: 0x0000294C
		public static double[] F28A6C2B(double[] 850B5909)
		{
			bool flag = 850B5909 == null || 850B5909.Length == 0;
			double[] result;
			if (flag)
			{
				result = new double[0];
			}
			else
			{
				double num = 850B5909[0];
				for (int i = 1; i < 850B5909.Length; i++)
				{
					bool flag2 = 850B5909[i] > num;
					if (flag2)
					{
						num = 850B5909[i];
					}
				}
				double[] array = new double[850B5909.Length];
				double num2 = 0.0;
				for (int j = 0; j < 850B5909.Length; j++)
				{
					array[j] = Math.Exp(850B5909[j] - num);
					num2 += array[j];
				}
				bool flag3 = num2 == 0.0 || double.IsNaN(num2) || double.IsInfinity(num2);
				if (flag3)
				{
					for (int k = 0; k < 850B5909.Length; k++)
					{
						array[k] = 1.0 / (double)850B5909.Length;
					}
					result = array;
				}
				else
				{
					for (int l = 0; l < 850B5909.Length; l++)
					{
						array[l] /= num2;
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004668 File Offset: 0x00002A68
		public static double[] 1FBA020D(double[] A5B2AE17, double[] 6884FF0F)
		{
			int num = A5B2AE17.Length;
			double[] array = new double[num];
			for (int i = 0; i < num; i++)
			{
				double num2 = 0.0;
				for (int j = 0; j < num; j++)
				{
					double num3 = (i == j) ? (A5B2AE17[i] * (1.0 - A5B2AE17[i])) : (-A5B2AE17[j] * A5B2AE17[i]);
					num2 += 6884FF0F[j] * num3;
				}
				array[i] = num2;
			}
			return array;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000046F0 File Offset: 0x00002AF0
		public static double[] 88BC3AB4(double[] 14A320B0, double[] B6ADFBAE)
		{
			bool flag = 14A320B0.Length != B6ADFBAE.Length;
			if (flag)
			{
				throw new ArgumentException("Vektör boyutları eşleşmeli.");
			}
			double[] array = new double[14A320B0.Length];
			for (int i = 0; i < 14A320B0.Length; i++)
			{
				array[i] = 14A320B0[i] + B6ADFBAE[i];
			}
			return array;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004744 File Offset: 0x00002B44
		public static List<double[]> DE1DDC21(List<double[]> E5230E10, List<double[]> 17BB6282)
		{
			bool flag = E5230E10.Count != 17BB6282.Count;
			if (flag)
			{
				throw new ArgumentException("Dizi uzunlukları eşleşmeli.");
			}
			List<double[]> list = new List<double[]>();
			for (int i = 0; i < E5230E10.Count; i++)
			{
				list.Add(469F9E21.C3259C27.88BC3AB4(E5230E10[i], 17BB6282[i]));
			}
			return list;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000047B0 File Offset: 0x00002BB0
		public static double FD91B9BA(double[] 4D94C59C, double[] 4E8B1702)
		{
			bool flag = 4D94C59C.Length != 4E8B1702.Length;
			if (flag)
			{
				throw new ArgumentException("Vektör boyutları eşleşmeli.");
			}
			double num = 0.0;
			for (int i = 0; i < 4D94C59C.Length; i++)
			{
				num += 4D94C59C[i] * 4E8B1702[i];
			}
			return num;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004808 File Offset: 0x00002C08
		public static double[,] 5C2F4812(List<double[]> 7FB6C5BC, List<double[]> DD0D8FB0)
		{
			int count = 7FB6C5BC.Count;
			int count2 = DD0D8FB0.Count;
			bool flag = count == 0 || count2 == 0;
			double[,] result;
			if (flag)
			{
				result = new double[count, count2];
			}
			else
			{
				int num = 7FB6C5BC[0].Length;
				bool flag2 = DD0D8FB0[0].Length == 0 && num == 0;
				if (!flag2)
				{
					bool flag3 = num != DD0D8FB0[0].Length;
					if (flag3)
					{
						throw new ArgumentException(string.Format("Q ({0}) ve K ({1}) özellik boyutları eşleşmeli.", num, DD0D8FB0[0].Length));
					}
				}
				double[,] array = new double[count, count2];
				for (int i = 0; i < count; i++)
				{
					for (int j = 0; j < count2; j++)
					{
						array[i, j] = 469F9E21.C3259C27.FD91B9BA(7FB6C5BC[i], DD0D8FB0[j]);
					}
				}
				result = array;
			}
			return result;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000048FC File Offset: 0x00002CFC
		public static List<double[]> 61189B89(double[,] 54ABD1B3, List<double[]> 441EC435)
		{
			int length = 54ABD1B3.GetLength(0);
			int length2 = 54ABD1B3.GetLength(1);
			bool flag = 441EC435.Count == 0 && length2 == 0;
			List<double[]> result;
			if (flag)
			{
				result = Enumerable.Range(0, length).Select(new Func<int, double[]>(469F9E21.C3259C27.EB367816.<>9.F0827AA8)).ToList<double[]>();
			}
			else
			{
				bool flag2 = length2 != 441EC435.Count;
				if (flag2)
				{
					throw new ArgumentException(string.Format("Attention skorları sütun ({0}) V listesi uzunluğuyla ({1}) eşleşmeli.", length2, 441EC435.Count));
				}
				List<double[]> list = new List<double[]>();
				int num = (441EC435.Count > 0) ? 441EC435[0].Length : 0;
				for (int i = 0; i < length; i++)
				{
					double[] array = new double[num];
					for (int j = 0; j < num; j++)
					{
						for (int k = 0; k < length2; k++)
						{
							array[j] += 54ABD1B3[i, k] * 441EC435[k][j];
						}
					}
					list.Add(array);
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004A38 File Offset: 0x00002E38
		public static double FA169D31(double A42B2FB1 = 0.0, double E50836BE = 1.0)
		{
			double d = 1.0 - 469F9E21.C3259C27.CB93CB80.NextDouble();
			double num = 1.0 - 469F9E21.C3259C27.CB93CB80.NextDouble();
			double num2 = Math.Sqrt(-2.0 * Math.Log(d)) * Math.Sin(6.283185307179586 * num);
			return A42B2FB1 + E50836BE * num2;
		}

		// Token: 0x04000040 RID: 64
		private static readonly Random CB93CB80 = new Random();

		// Token: 0x0200001E RID: 30
		[CompilerGenerated]
		[Serializable]
		private sealed class EB367816
		{
			// Token: 0x060000CD RID: 205 RVA: 0x000088AD File Offset: 0x00006CAD
			internal double[] F0827AA8(int AF0E2390)
			{
				return new double[0];
			}

			// Token: 0x0400008C RID: 140
			public static readonly 469F9E21.C3259C27.EB367816 <>9 = new 469F9E21.C3259C27.EB367816();

			// Token: 0x0400008D RID: 141
			public static Func<int, double[]> <>9__7_0;
		}
	}

	// Token: 0x02000011 RID: 17
	public class 14211E00 : 469F9E21.FD81572E
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00004AAC File Offset: 0x00002EAC
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00004AB4 File Offset: 0x00002EB4
		public string 9DA5C617 { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00004ABD File Offset: 0x00002EBD
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00004AC5 File Offset: 0x00002EC5
		public double[,] 2221D729 { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00004ACE File Offset: 0x00002ECE
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00004AD6 File Offset: 0x00002ED6
		public double[] AEB1B0AB { get; private set; }

		// Token: 0x06000070 RID: 112 RVA: 0x00004AE0 File Offset: 0x00002EE0
		public 14211E00(int 7589AC22, int E623BF29, string 44127EBC)
		{
			this.A88F111F = 7589AC22;
			this.8A2A02AB = E623BF29;
			this.9DA5C617 = 44127EBC;
			this.2221D729 = new double[this.A88F111F, this.8A2A02AB];
			this.AEB1B0AB = new double[this.8A2A02AB];
			double e50836BE = Math.Sqrt(2.0 / (double)(this.A88F111F + Math.Max(1, this.8A2A02AB)));
			bool flag = this.A88F111F == 0;
			if (flag)
			{
				e50836BE = Math.Sqrt(2.0 / (double)Math.Max(1, this.8A2A02AB));
			}
			for (int i = 0; i < this.A88F111F; i++)
			{
				for (int j = 0; j < this.8A2A02AB; j++)
				{
					this.2221D729[i, j] = 469F9E21.C3259C27.FA169D31(0.0, e50836BE);
				}
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004BD0 File Offset: 0x00002FD0
		public override double[] 7B132382(double[] 7E36209D, bool 2E99241F = false)
		{
			bool flag = 7E36209D.Length != this.A88F111F;
			if (flag)
			{
				throw new ArgumentException(string.Format("DenseLayer.Forward: Beklenen giriş boyutu {0}, gelen {1}", this.A88F111F, 7E36209D.Length));
			}
			this.FA2512A0 = (double[])7E36209D.Clone();
			this.B017599D = new double[this.8A2A02AB];
			for (int i = 0; i < this.8A2A02AB; i++)
			{
				double num = this.AEB1B0AB[i];
				for (int j = 0; j < this.A88F111F; j++)
				{
					num += 7E36209D[j] * this.2221D729[j, i];
				}
				this.B017599D[i] = num;
			}
			this.C9B7270A = new double[this.8A2A02AB];
			bool flag2 = this.9DA5C617 == "ActivationRamp";
			if (flag2)
			{
				for (int k = 0; k < this.8A2A02AB; k++)
				{
					this.C9B7270A[k] = Math.Max(0.0, this.B017599D[k]);
				}
			}
			else
			{
				bool flag3 = this.9DA5C617 == "ActivationHyperbolic";
				if (flag3)
				{
					for (int l = 0; l < this.8A2A02AB; l++)
					{
						this.C9B7270A[l] = Math.Tanh(this.B017599D[l]);
					}
				}
				else
				{
					bool flag4 = string.IsNullOrEmpty(this.9DA5C617) || this.9DA5C617.ToLower() == "activationidentity" || this.9DA5C617.ToLower() == "linear";
					if (!flag4)
					{
						throw new ArgumentException("Bilinmeyen aktivasyon fonksiyonu: " + this.9DA5C617);
					}
					this.C9B7270A = (double[])this.B017599D.Clone();
				}
			}
			return (double[])this.C9B7270A.Clone();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004DC4 File Offset: 0x000031C4
		public override double[] 721B1D97(double[] 88976E8F, double 1F24C6AC)
		{
			ValueTuple<double[], double[,], double[]> valueTuple = this.F6B0FEB1(88976E8F);
			double[] item = valueTuple.Item1;
			double[,] item2 = valueTuple.Item2;
			double[] item3 = valueTuple.Item3;
			bool flag = 1F24C6AC > 0.0;
			if (flag)
			{
				this.208A2188(item2, item3, 1F24C6AC);
			}
			return item;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004E10 File Offset: 0x00003210
		[return: TupleElementNames(new string[]
		{
			"errorForPrevLayer",
			"dL_dW",
			"dL_dB"
		})]
		public ValueTuple<double[], double[,], double[]> F6B0FEB1(double[] 4A85F728)
		{
			double[] array = new double[this.8A2A02AB];
			for (int i = 0; i < this.8A2A02AB; i++)
			{
				double num;
				if (!(this.9DA5C617 == "ActivationRamp"))
				{
					if (!(this.9DA5C617 == "ActivationHyperbolic"))
					{
						if (!string.IsNullOrEmpty(this.9DA5C617) && !(this.9DA5C617.ToLower() == "activationidentity") && !(this.9DA5C617.ToLower() == "linear"))
						{
							throw new ArgumentException("Bilinmeyen aktivasyon: " + this.9DA5C617);
						}
						num = 1.0;
					}
					else
					{
						num = 1.0 - this.C9B7270A[i] * this.C9B7270A[i];
					}
				}
				else
				{
					num = ((this.B017599D[i] > 0.0) ? 1.0 : 0.0);
				}
				double num2 = num;
				array[i] = 4A85F728[i] * num2;
			}
			double[,] array2 = new double[this.A88F111F, this.8A2A02AB];
			double[] item = (double[])array.Clone();
			for (int j = 0; j < this.8A2A02AB; j++)
			{
				for (int k = 0; k < this.A88F111F; k++)
				{
					array2[k, j] = this.FA2512A0[k] * array[j];
				}
			}
			double[] array3 = new double[this.A88F111F];
			for (int l = 0; l < this.A88F111F; l++)
			{
				double num3 = 0.0;
				for (int m = 0; m < this.8A2A02AB; m++)
				{
					num3 += array[m] * this.2221D729[l, m];
				}
				array3[l] = num3;
			}
			return new ValueTuple<double[], double[,], double[]>(array3, array2, item);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000500C File Offset: 0x0000340C
		public void 208A2188(double[,] A680A02C, double[] 779095B7, double 360A9F9A)
		{
			for (int i = 0; i < this.8A2A02AB; i++)
			{
				this.AEB1B0AB[i] -= 360A9F9A * 779095B7[i];
				for (int j = 0; j < this.A88F111F; j++)
				{
					this.2221D729[j, i] -= 360A9F9A * A680A02C[j, i];
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005078 File Offset: 0x00003478
		public override object 3A811634()
		{
			double[][] array = new double[this.A88F111F][];
			for (int i = 0; i < this.A88F111F; i++)
			{
				array[i] = new double[this.8A2A02AB];
				for (int j = 0; j < this.8A2A02AB; j++)
				{
					array[i][j] = this.2221D729[i, j];
				}
			}
			return new E115D427<string, int, int, string, double[][], double[]>("DenseLayer", this.A88F111F, this.8A2A02AB, this.9DA5C617, array, this.AEB1B0AB);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000510C File Offset: 0x0000350C
		public override void 4486B5B7(JsonElement DD0EDDAA)
		{
			this.9DA5C617 = DD0EDDAA.GetProperty("activation").GetString();
			List<JsonElement> list = DD0EDDAA.GetProperty("W").EnumerateArray().ToList<JsonElement>();
			for (int i = 0; i < this.A88F111F; i++)
			{
				List<JsonElement> list2 = list[i].EnumerateArray().ToList<JsonElement>();
				for (int j = 0; j < this.8A2A02AB; j++)
				{
					this.2221D729[i, j] = list2[j].GetDouble();
				}
			}
			List<JsonElement> list3 = DD0EDDAA.GetProperty("B").EnumerateArray().ToList<JsonElement>();
			for (int k = 0; k < this.8A2A02AB; k++)
			{
				this.AEB1B0AB[k] = list3[k].GetDouble();
			}
		}

		// Token: 0x04000041 RID: 65
		private readonly int A88F111F;

		// Token: 0x04000042 RID: 66
		private readonly int 8A2A02AB;

		// Token: 0x04000043 RID: 67
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string B9264B34;

		// Token: 0x04000044 RID: 68
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private double[,] E6373508;

		// Token: 0x04000045 RID: 69
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private double[] 7C00FB07;

		// Token: 0x04000046 RID: 70
		private double[] FA2512A0;

		// Token: 0x04000047 RID: 71
		private double[] B017599D;

		// Token: 0x04000048 RID: 72
		private double[] C9B7270A;
	}

	// Token: 0x02000012 RID: 18
	public class 8482B4BA : 469F9E21.FD81572E
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00005215 File Offset: 0x00003615
		public double D715CF35 { get; }

		// Token: 0x06000078 RID: 120 RVA: 0x0000521D File Offset: 0x0000361D
		public 8482B4BA(double 751C573D)
		{
			this.D715CF35 = 751C573D;
			this.419F6504 = 1.0 - 751C573D;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005240 File Offset: 0x00003640
		public override double[] 7B132382(double[] A6078D96, bool 37200E03 = false)
		{
			bool flag = !37200E03 || this.D715CF35 == 0.0;
			double[] result;
			if (flag)
			{
				this.8C24CCAF = null;
				result = A6078D96;
			}
			else
			{
				this.8C24CCAF = new bool[A6078D96.Length];
				double[] array = new double[A6078D96.Length];
				for (int i = 0; i < A6078D96.Length; i++)
				{
					this.8C24CCAF[i] = (469F9E21.8482B4BA.FD0A3411.NextDouble() < this.419F6504);
					array[i] = (this.8C24CCAF[i] ? (A6078D96[i] / this.419F6504) : 0.0);
				}
				result = array;
			}
			return result;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000052E4 File Offset: 0x000036E4
		public override double[] 721B1D97(double[] 14113300, double 120B4A1B)
		{
			bool flag = this.8C24CCAF == null || this.D715CF35 == 0.0;
			double[] result;
			if (flag)
			{
				result = 14113300;
			}
			else
			{
				double[] array = new double[14113300.Length];
				for (int i = 0; i < 14113300.Length; i++)
				{
					array[i] = (this.8C24CCAF[i] ? (14113300[i] / this.419F6504) : 0.0);
				}
				result = array;
			}
			return result;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000535A File Offset: 0x0000375A
		public override object 3A811634()
		{
			return new E8B82097<string, double>("DropoutLayer", this.D715CF35);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000536C File Offset: 0x0000376C
		public override void 4486B5B7(JsonElement 3E8C9316)
		{
		}

		// Token: 0x04000049 RID: 73
		private readonly double 419F6504;

		// Token: 0x0400004A RID: 74
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly double F199F626;

		// Token: 0x0400004B RID: 75
		private bool[] 8C24CCAF;

		// Token: 0x0400004C RID: 76
		private static readonly Random FD0A3411 = new Random();
	}

	// Token: 0x02000013 RID: 19
	public class 0C9957BA
	{
		// Token: 0x0600007E RID: 126 RVA: 0x0000537C File Offset: 0x0000377C
		public 0C9957BA(int 31BC8A86, int 78003487)
		{
			this.3DB9F810 = 31BC8A86;
			this.549E2130 = 78003487;
			this.D69C360E = new double[78003487, 31BC8A86];
			for (int i = 0; i < 78003487; i++)
			{
				for (int j = 0; j < 31BC8A86; j++)
				{
					double num = Math.Pow(10000.0, (double)(2 * (j / 2)) / (double)31BC8A86);
					this.D69C360E[i, j] = ((j % 2 == 0) ? Math.Sin((double)i / num) : Math.Cos((double)i / num));
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005410 File Offset: 0x00003810
		public List<double[]> 4A1BEA80(List<double[]> C70320A7)
		{
			int count = C70320A7.Count;
			bool flag = count > this.549E2130;
			if (flag)
			{
				throw new ArgumentException(string.Format("Dizi uzunluğu ({0}) > max izin verilen ({1}).", count, this.549E2130));
			}
			List<double[]> list = new List<double[]>();
			for (int i = 0; i < count; i++)
			{
				double[] array = new double[this.3DB9F810];
				for (int j = 0; j < this.3DB9F810; j++)
				{
					array[j] = C70320A7[i][j] + this.D69C360E[i, j];
				}
				list.Add(array);
			}
			return list;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000054C3 File Offset: 0x000038C3
		public List<double[]> FE35123B(List<double[]> 4DAF0C25)
		{
			return 4DAF0C25;
		}

		// Token: 0x0400004D RID: 77
		private readonly int 3DB9F810;

		// Token: 0x0400004E RID: 78
		private readonly int 549E2130;

		// Token: 0x0400004F RID: 79
		private readonly double[,] D69C360E;
	}

	// Token: 0x02000014 RID: 20
	public class 30907DB3 : 469F9E21.FD81572E
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000054C6 File Offset: 0x000038C6
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000054CE File Offset: 0x000038CE
		public double[] 6E265C3F { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000054D7 File Offset: 0x000038D7
		// (set) Token: 0x06000084 RID: 132 RVA: 0x000054DF File Offset: 0x000038DF
		public double[] 94BEEAA0 { get; private set; }

		// Token: 0x06000085 RID: 133 RVA: 0x000054E8 File Offset: 0x000038E8
		public 30907DB3(int 4587B4A3)
		{
			this.D29B0195 = 4587B4A3;
			this.6E265C3F = new double[4587B4A3];
			this.94BEEAA0 = new double[4587B4A3];
			for (int i = 0; i < 4587B4A3; i++)
			{
				this.6E265C3F[i] = 1.0;
				this.94BEEAA0[i] = 0.0;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005564 File Offset: 0x00003964
		public override double[] 7B132382(double[] 9A8E218B, bool 7514E020 = false)
		{
			bool flag = 9A8E218B.Length != this.D29B0195;
			if (flag)
			{
				throw new ArgumentException(string.Format("LayerNorm: Giriş boyutu {0}, gelen {1}", this.D29B0195, 9A8E218B.Length));
			}
			this.A5158591 = (double[])9A8E218B.Clone();
			this.AA10AA1B = 9A8E218B.Average();
			this.C29BD6AB = 9A8E218B.Select(new Func<double, double>(this.C9363D84)).Sum() / (double)this.D29B0195;
			this.3A8EC081 = 1.0 / Math.Sqrt(this.C29BD6AB + this.CD85003F);
			this.1B3C4985 = new double[this.D29B0195];
			for (int i = 0; i < this.D29B0195; i++)
			{
				this.1B3C4985[i] = (9A8E218B[i] - this.AA10AA1B) * this.3A8EC081;
			}
			double[] array = new double[this.D29B0195];
			for (int j = 0; j < this.D29B0195; j++)
			{
				array[j] = this.6E265C3F[j] * this.1B3C4985[j] + this.94BEEAA0[j];
			}
			return array;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000569C File Offset: 0x00003A9C
		public override double[] 721B1D97(double[] 29221D00, double EAAC51A6)
		{
			double[] array = (double[])29221D00.Clone();
			double[] array2 = new double[this.D29B0195];
			for (int i = 0; i < this.D29B0195; i++)
			{
				array2[i] = 29221D00[i] * this.1B3C4985[i];
			}
			double[] array3 = new double[this.D29B0195];
			for (int j = 0; j < this.D29B0195; j++)
			{
				array3[j] = 29221D00[j] * this.6E265C3F[j];
			}
			double num = 0.0;
			for (int k = 0; k < this.D29B0195; k++)
			{
				num += array3[k] * (this.A5158591[k] - this.AA10AA1B);
			}
			double num2 = num * (-0.5 * Math.Pow(this.3A8EC081, 3.0));
			double num3 = 0.0;
			for (int l = 0; l < this.D29B0195; l++)
			{
				num3 += array3[l] * -this.3A8EC081;
			}
			double num4 = 0.0;
			for (int m = 0; m < this.D29B0195; m++)
			{
				num4 += this.A5158591[m] - this.AA10AA1B;
			}
			double num5 = num2 * (-2.0 * num4 / (double)this.D29B0195);
			double num6 = num3 + num5;
			double[] array4 = new double[this.D29B0195];
			for (int n = 0; n < this.D29B0195; n++)
			{
				array4[n] = array3[n] * this.3A8EC081 + num2 * (2.0 * (this.A5158591[n] - this.AA10AA1B)) / (double)this.D29B0195 + num6 / (double)this.D29B0195;
			}
			bool flag = EAAC51A6 > 0.0;
			if (flag)
			{
				for (int num7 = 0; num7 < this.D29B0195; num7++)
				{
					this.6E265C3F[num7] -= EAAC51A6 * array2[num7];
					this.94BEEAA0[num7] -= EAAC51A6 * array[num7];
				}
			}
			return array4;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000058F6 File Offset: 0x00003CF6
		public override object 3A811634()
		{
			return new C53BD307<string, int, double[], double[]>("LayerNormalization", this.D29B0195, this.6E265C3F, this.94BEEAA0);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005914 File Offset: 0x00003D14
		public override void 4486B5B7(JsonElement E623FD07)
		{
			JsonElement[] array = E623FD07.GetProperty("Gamma").EnumerateArray().ToArray<JsonElement>();
			JsonElement[] array2 = E623FD07.GetProperty("Beta").EnumerateArray().ToArray<JsonElement>();
			for (int i = 0; i < this.D29B0195; i++)
			{
				this.6E265C3F[i] = array[i].GetDouble();
				this.94BEEAA0[i] = array2[i].GetDouble();
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000059A1 File Offset: 0x00003DA1
		[CompilerGenerated]
		private double C9363D84(double 8685A1BF)
		{
			return (8685A1BF - this.AA10AA1B) * (8685A1BF - this.AA10AA1B);
		}

		// Token: 0x04000050 RID: 80
		private readonly int D29B0195;

		// Token: 0x04000051 RID: 81
		private readonly double CD85003F = 1E-06;

		// Token: 0x04000052 RID: 82
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private double[] F220B9AA;

		// Token: 0x04000053 RID: 83
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private double[] 51826A82;

		// Token: 0x04000054 RID: 84
		private double[] A5158591;

		// Token: 0x04000055 RID: 85
		private double AA10AA1B;

		// Token: 0x04000056 RID: 86
		private double C29BD6AB;

		// Token: 0x04000057 RID: 87
		private double 3A8EC081;

		// Token: 0x04000058 RID: 88
		private double[] 1B3C4985;
	}

	// Token: 0x02000015 RID: 21
	public static class 0009DD17
	{
		// Token: 0x0600008B RID: 139 RVA: 0x000059B4 File Offset: 0x00003DB4
		[return: TupleElementNames(new string[]
		{
			"output",
			"attentionWeights",
			"Q_proj",
			"K_proj",
			"V_proj"
		})]
		public static ValueTuple<List<double[]>, double[,], List<double[]>, List<double[]>, List<double[]>> FC910EBF(List<double[]> FC9CF609, List<double[]> 2B190B96, List<double[]> A4B9BF10, 469F9E21.14211E00 B7AFA88D, 469F9E21.14211E00 ECAF90AD, 469F9E21.14211E00 DF1F3B9D, bool 6C031A85 = false, bool C28806A4 = false)
		{
			469F9E21.0009DD17.19A711A9 19A711A = new 469F9E21.0009DD17.19A711A9();
			19A711A.D92E6B16 = B7AFA88D;
			19A711A.68A3F481 = C28806A4;
			19A711A.0A32CF99 = ECAF90AD;
			19A711A.3E8FE184 = DF1F3B9D;
			List<double[]> list = FC9CF609.Select(new Func<double[], double[]>(19A711A.3BB80296)).ToList<double[]>();
			List<double[]> list2 = 2B190B96.Select(new Func<double[], double[]>(19A711A.1B15F30D)).ToList<double[]>();
			List<double[]> list3 = A4B9BF10.Select(new Func<double[], double[]>(19A711A.EA144F29)).ToList<double[]>();
			int count = list.Count;
			int count2 = list2.Count;
			bool flag = count2 == 0;
			ValueTuple<List<double[]>, double[,], List<double[]>, List<double[]>, List<double[]>> result;
			if (flag)
			{
				469F9E21.0009DD17.0338F49E 0338F49E = new 469F9E21.0009DD17.0338F49E();
				0338F49E.F93C1696 = ((A4B9BF10.Count > 0 && A4B9BF10[0].Length != 0) ? A4B9BF10[0].Length : ((list3.Count > 0 && list3[0].Length != 0) ? list3[0].Length : 0));
				bool flag2 = list3.Count > 0 && list3[0].Length != 0;
				if (flag2)
				{
					0338F49E.F93C1696 = list3[0].Length;
				}
				List<double[]> item = list.Select(new Func<double[], double[]>(0338F49E.0933CB31)).ToList<double[]>();
				result = new ValueTuple<List<double[]>, double[,], List<double[]>, List<double[]>, List<double[]>>(item, new double[count, count2], list, list2, list3);
			}
			else
			{
				int num = list2[0].Length;
				bool flag3 = num == 0 && count2 > 0;
				if (flag3)
				{
					throw new ArgumentException("d_k sıfır olamaz (K projeksiyonu sonrası ve K boş değilse).");
				}
				bool flag4 = num == 0 && count2 == 0;
				if (flag4)
				{
				}
				double[,] array = 469F9E21.C3259C27.5C2F4812(list, list2);
				double num2 = (num > 0) ? (1.0 / Math.Sqrt((double)num)) : 1.0;
				for (int i = 0; i < count; i++)
				{
					for (int j = 0; j < count2; j++)
					{
						array[i, j] *= num2;
					}
				}
				if (6C031A85)
				{
					for (int k = 0; k < count; k++)
					{
						for (int l = 0; l < count2; l++)
						{
							bool flag5 = l > k;
							if (flag5)
							{
								array[k, l] = double.NegativeInfinity;
							}
						}
					}
				}
				double[,] array2 = new double[count, count2];
				for (int m = 0; m < count; m++)
				{
					double[] array3 = new double[count2];
					for (int n = 0; n < count2; n++)
					{
						array3[n] = array[m, n];
					}
					double[] array4 = 469F9E21.C3259C27.F28A6C2B(array3);
					for (int num3 = 0; num3 < count2; num3++)
					{
						array2[m, num3] = array4[num3];
					}
				}
				List<double[]> item2 = 469F9E21.C3259C27.61189B89(array2, list3);
				result = new ValueTuple<List<double[]>, double[,], List<double[]>, List<double[]>, List<double[]>>(item2, array2, list, list2, list3);
			}
			return result;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005C98 File Offset: 0x00004098
		[return: TupleElementNames(new string[]
		{
			"dL_dQ_proj",
			"dL_dK_proj",
			"dL_dV_proj"
		})]
		public static ValueTuple<List<double[]>, List<double[]>, List<double[]>> FD258E34(List<double[]> 31953C08, double[,] 0496662F, List<double[]> 8D120A1E, List<double[]> 7983A412, List<double[]> 4AB1C13E, double 601066AE)
		{
			int count = 31953C08.Count;
			bool flag = count == 0;
			ValueTuple<List<double[]>, List<double[]>, List<double[]>> result;
			if (flag)
			{
				result = new ValueTuple<List<double[]>, List<double[]>, List<double[]>>(new List<double[]>(), new List<double[]>(), new List<double[]>());
			}
			else
			{
				int num = (31953C08[0] != null) ? 31953C08[0].Length : 0;
				int count2 = 7983A412.Count;
				bool flag2 = count2 == 0;
				if (flag2)
				{
					result = new ValueTuple<List<double[]>, List<double[]>, List<double[]>>(8D120A1E.Select(new Func<double[], double[]>(469F9E21.0009DD17.DE91D096.<>9.C6183C24)).ToList<double[]>(), 7983A412.Select(new Func<double[], double[]>(469F9E21.0009DD17.DE91D096.<>9.A9887897)).ToList<double[]>(), 4AB1C13E.Select(new Func<double[], double[]>(469F9E21.0009DD17.DE91D096.<>9.41189792)).ToList<double[]>());
				}
				else
				{
					int num2 = 7983A412[0].Length;
					bool flag3 = num2 == 0 && count2 > 0;
					if (flag3)
					{
						throw new InvalidOperationException("K_proj[0].Length sıfır olamaz eğer Lk > 0.");
					}
					List<double[]> list = new List<double[]>(count2);
					for (int i = 0; i < count2; i++)
					{
						double[] array = new double[num];
						for (int j = 0; j < num; j++)
						{
							for (int k = 0; k < count; k++)
							{
								array[j] += 31953C08[k][j] * 0496662F[k, i];
							}
						}
						list.Add(array);
					}
					double[,] array2 = new double[count, count2];
					for (int l = 0; l < count; l++)
					{
						for (int m = 0; m < count2; m++)
						{
							bool flag4 = 4AB1C13E.Count > m && 4AB1C13E[m] != null;
							if (flag4)
							{
								array2[l, m] = 469F9E21.C3259C27.FD91B9BA(31953C08[l], 4AB1C13E[m]);
							}
							else
							{
								array2[l, m] = 0.0;
							}
						}
					}
					double[,] array3 = new double[count, count2];
					for (int n = 0; n < count; n++)
					{
						double[] array4 = new double[count2];
						double[] array5 = new double[count2];
						for (int num3 = 0; num3 < count2; num3++)
						{
							array4[num3] = 0496662F[n, num3];
							array5[num3] = array2[n, num3];
						}
						double[] array6 = 469F9E21.C3259C27.1FBA020D(array4, array5);
						for (int num4 = 0; num4 < count2; num4++)
						{
							array3[n, num4] = array6[num4];
						}
					}
					double[,] array7 = new double[count, count2];
					for (int num5 = 0; num5 < count; num5++)
					{
						for (int num6 = 0; num6 < count2; num6++)
						{
							array7[num5, num6] = array3[num5, num6] * 601066AE;
						}
					}
					List<double[]> list2 = new List<double[]>(count);
					for (int num7 = 0; num7 < count; num7++)
					{
						double[] array8 = new double[num2];
						bool flag5 = num2 > 0;
						if (flag5)
						{
							for (int num8 = 0; num8 < num2; num8++)
							{
								for (int num9 = 0; num9 < count2; num9++)
								{
									array8[num8] += array7[num7, num9] * 7983A412[num9][num8];
								}
							}
						}
						list2.Add(array8);
					}
					List<double[]> list3 = new List<double[]>(count2);
					for (int num10 = 0; num10 < count2; num10++)
					{
						double[] array9 = new double[num2];
						bool flag6 = num2 > 0;
						if (flag6)
						{
							for (int num11 = 0; num11 < num2; num11++)
							{
								for (int num12 = 0; num12 < count; num12++)
								{
									array9[num11] += array7[num12, num10] * 8D120A1E[num12][num11];
								}
							}
						}
						list3.Add(array9);
					}
					result = new ValueTuple<List<double[]>, List<double[]>, List<double[]>>(list2, list3, list);
				}
			}
			return result;
		}

		// Token: 0x0200001F RID: 31
		[CompilerGenerated]
		[Serializable]
		private sealed class DE91D096
		{
			// Token: 0x060000D0 RID: 208 RVA: 0x000088CA File Offset: 0x00006CCA
			internal double[] C6183C24(double[] C02A2217)
			{
				return new double[C02A2217.Length];
			}

			// Token: 0x060000D1 RID: 209 RVA: 0x000088D4 File Offset: 0x00006CD4
			internal double[] A9887897(double[] 22A95B2C)
			{
				return new double[22A95B2C.Length];
			}

			// Token: 0x060000D2 RID: 210 RVA: 0x000088DE File Offset: 0x00006CDE
			internal double[] 41189792(double[] F6081C05)
			{
				return new double[F6081C05.Length];
			}

			// Token: 0x0400008E RID: 142
			public static readonly 469F9E21.0009DD17.DE91D096 <>9 = new 469F9E21.0009DD17.DE91D096();

			// Token: 0x0400008F RID: 143
			public static Func<double[], double[]> <>9__1_0;

			// Token: 0x04000090 RID: 144
			public static Func<double[], double[]> <>9__1_1;

			// Token: 0x04000091 RID: 145
			public static Func<double[], double[]> <>9__1_2;
		}

		// Token: 0x02000020 RID: 32
		[CompilerGenerated]
		private sealed class 19A711A9
		{
			// Token: 0x060000D4 RID: 212 RVA: 0x000088F1 File Offset: 0x00006CF1
			internal double[] 3BB80296(double[] FD3AEA84)
			{
				return this.D92E6B16.7B132382(FD3AEA84, this.68A3F481);
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x00008905 File Offset: 0x00006D05
			internal double[] 1B15F30D(double[] B7AE8EB2)
			{
				return this.0A32CF99.7B132382(B7AE8EB2, this.68A3F481);
			}

			// Token: 0x060000D6 RID: 214 RVA: 0x00008919 File Offset: 0x00006D19
			internal double[] EA144F29(double[] ED3AFBBD)
			{
				return this.3E8FE184.7B132382(ED3AFBBD, this.68A3F481);
			}

			// Token: 0x04000092 RID: 146
			public 469F9E21.14211E00 D92E6B16;

			// Token: 0x04000093 RID: 147
			public bool 68A3F481;

			// Token: 0x04000094 RID: 148
			public 469F9E21.14211E00 0A32CF99;

			// Token: 0x04000095 RID: 149
			public 469F9E21.14211E00 3E8FE184;
		}

		// Token: 0x02000021 RID: 33
		[CompilerGenerated]
		private sealed class 0338F49E
		{
			// Token: 0x060000D8 RID: 216 RVA: 0x00008936 File Offset: 0x00006D36
			internal double[] 0933CB31(double[] F233F7BE)
			{
				return new double[this.F93C1696];
			}

			// Token: 0x04000096 RID: 150
			public int F93C1696;
		}
	}

	// Token: 0x02000016 RID: 22
	public class 78B3B48A
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000060DC File Offset: 0x000044DC
		// (set) Token: 0x0600008E RID: 142 RVA: 0x000060E4 File Offset: 0x000044E4
		public List<469F9E21.14211E00> B03A8F31 { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000060ED File Offset: 0x000044ED
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000060F5 File Offset: 0x000044F5
		public List<469F9E21.14211E00> 9F973697 { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000060FE File Offset: 0x000044FE
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00006106 File Offset: 0x00004506
		public List<469F9E21.14211E00> 28AE4502 { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000610F File Offset: 0x0000450F
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00006117 File Offset: 0x00004517
		public 469F9E21.14211E00 21809C32 { get; private set; }

		// Token: 0x06000095 RID: 149 RVA: 0x00006120 File Offset: 0x00004520
		public 78B3B48A(int B108941E, int DDA67214)
		{
			469F9E21.78B3B48A.EB86542F eb86542F = new 469F9E21.78B3B48A.EB86542F();
			eb86542F.CF9B6899 = B108941E;
			base..ctor();
			eb86542F.15BA9327 = this;
			bool flag = eb86542F.CF9B6899 % DDA67214 != 0;
			if (flag)
			{
				throw new ArgumentException("dModel, numHeads'e tam bölünmelidir.");
			}
			this.24884523 = eb86542F.CF9B6899;
			this.288DC303 = DDA67214;
			this.892E4FA9 = eb86542F.CF9B6899 / DDA67214;
			this.6305A81C = eb86542F.CF9B6899 / DDA67214;
			this.B03A8F31 = Enumerable.Range(0, DDA67214).Select(new Func<int, 469F9E21.14211E00>(eb86542F.0CB2538C)).ToList<469F9E21.14211E00>();
			this.9F973697 = Enumerable.Range(0, DDA67214).Select(new Func<int, 469F9E21.14211E00>(eb86542F.1887E238)).ToList<469F9E21.14211E00>();
			this.28AE4502 = Enumerable.Range(0, DDA67214).Select(new Func<int, 469F9E21.14211E00>(eb86542F.5CA02732)).ToList<469F9E21.14211E00>();
			this.21809C32 = new 469F9E21.14211E00(DDA67214 * this.6305A81C, eb86542F.CF9B6899, "ActivationIdentity");
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006220 File Offset: 0x00004620
		public List<double[]> C83DD887(List<double[]> D7ACA8BF, bool 980B743A = false)
		{
			469F9E21.78B3B48A.071D6092 071D = new 469F9E21.78B3B48A.071D6092();
			071D.5AAD01B1 = this;
			071D.60989102 = 980B743A;
			this.BFA3B628 = D7ACA8BF.Select(new Func<double[], double[]>(469F9E21.78B3B48A.E00D94AC.<>9.E917C49A)).ToList<double[]>();
			int count = D7ACA8BF.Count;
			List<List<double[]>> list = new List<List<double[]>>(this.288DC303);
			this.D133A931 = new List<List<double[]>>(this.288DC303);
			this.53B3C70D = new List<List<double[]>>(this.288DC303);
			this.5B04398F = new List<List<double[]>>(this.288DC303);
			this.C60F14BA = new List<double[,]>(this.288DC303);
			for (int i = 0; i < this.288DC303; i++)
			{
				ValueTuple<List<double[]>, double[,], List<double[]>, List<double[]>, List<double[]>> valueTuple = 469F9E21.0009DD17.FC910EBF(D7ACA8BF, D7ACA8BF, D7ACA8BF, this.B03A8F31[i], this.9F973697[i], this.28AE4502[i], false, 071D.60989102);
				List<double[]> item = valueTuple.Item1;
				double[,] item2 = valueTuple.Item2;
				List<double[]> item3 = valueTuple.Item3;
				List<double[]> item4 = valueTuple.Item4;
				List<double[]> item5 = valueTuple.Item5;
				list.Add(item);
				this.D133A931.Add(item3);
				this.53B3C70D.Add(item4);
				this.5B04398F.Add(item5);
				this.C60F14BA.Add(item2);
			}
			List<double[]> list2 = new List<double[]>(count);
			for (int j = 0; j < count; j++)
			{
				List<double> list3 = new List<double>(this.288DC303 * this.6305A81C);
				for (int k = 0; k < this.288DC303; k++)
				{
					bool flag = list[k].Count > j;
					if (flag)
					{
						list3.AddRange(list[k][j]);
					}
					else
					{
						bool flag2 = this.6305A81C > 0;
						if (flag2)
						{
							list3.AddRange(new double[this.6305A81C]);
						}
					}
				}
				list2.Add(list3.ToArray());
			}
			this.2AAC70B9 = list2.Select(new Func<double[], double[]>(469F9E21.78B3B48A.E00D94AC.<>9.2D934B8D)).ToList<double[]>();
			return this.2AAC70B9.Select(new Func<double[], double[]>(071D.7A1F3486)).ToList<double[]>();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006494 File Offset: 0x00004894
		public List<double[]> F8A5CDA6(List<double[]> 55B2D6A6, double F80F821F)
		{
			int count = 55B2D6A6.Count;
			List<double[]> list = Enumerable.Range(0, count).Select(new Func<int, double[]>(this.D91D208D)).ToList<double[]>();
			List<double[]> list2 = new List<double[]>(count);
			List<double[,]> list3 = new List<double[,]>();
			List<double[]> list4 = new List<double[]>();
			for (int i = 0; i < count; i++)
			{
				this.21809C32.7B132382(this.2AAC70B9[i], true);
				ValueTuple<double[], double[,], double[]> valueTuple = this.21809C32.F6B0FEB1(55B2D6A6[i]);
				double[] item = valueTuple.Item1;
				double[,] item2 = valueTuple.Item2;
				double[] item3 = valueTuple.Item3;
				list2.Add(item);
				list3.Add(item2);
				list4.Add(item3);
			}
			bool flag;
			if (F80F821F > 0.0)
			{
				flag = list3.Any(new Func<double[,], bool>(469F9E21.78B3B48A.E00D94AC.<>9.3E9D04A1));
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				double[,] array = this.85BA0CA1(list3.Where(new Func<double[,], bool>(469F9E21.78B3B48A.E00D94AC.<>9.0A14AC08)).ToList<double[,]>());
				double[] array2 = this.2996BC9E(list4.Where(new Func<double[], bool>(469F9E21.78B3B48A.E00D94AC.<>9.433B13B8)).ToList<double[]>());
				bool flag3 = array != null && array2 != null;
				if (flag3)
				{
					this.21809C32.208A2188(array, array2, F80F821F);
				}
			}
			for (int j = 0; j < this.288DC303; j++)
			{
				List<double[]> list5 = new List<double[]>(count);
				for (int k = 0; k < count; k++)
				{
					double[] array3 = new double[this.6305A81C];
					bool flag4 = list2[k].Length >= (j + 1) * this.6305A81C;
					if (flag4)
					{
						Array.Copy(list2[k], j * this.6305A81C, array3, 0, this.6305A81C);
					}
					list5.Add(array3);
				}
				double 601066AE = (this.892E4FA9 > 0) ? (1.0 / Math.Sqrt((double)this.892E4FA9)) : 1.0;
				ValueTuple<List<double[]>, List<double[]>, List<double[]>> valueTuple2 = 469F9E21.0009DD17.FD258E34(list5, this.C60F14BA[j], this.D133A931[j], this.53B3C70D[j], this.5B04398F[j], 601066AE);
				List<double[]> item4 = valueTuple2.Item1;
				List<double[]> item5 = valueTuple2.Item2;
				List<double[]> item6 = valueTuple2.Item3;
				List<double[]> list6 = this.AE859F07(item4, this.B03A8F31[j], this.BFA3B628, F80F821F);
				List<double[]> list7 = this.AE859F07(item5, this.9F973697[j], this.BFA3B628, F80F821F);
				List<double[]> list8 = this.AE859F07(item6, this.28AE4502[j], this.BFA3B628, F80F821F);
				for (int l = 0; l < count; l++)
				{
					list[l] = 469F9E21.C3259C27.88BC3AB4(list[l], list6[l]);
					list[l] = 469F9E21.C3259C27.88BC3AB4(list[l], list7[l]);
					list[l] = 469F9E21.C3259C27.88BC3AB4(list[l], list8[l]);
				}
			}
			return list;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000680C File Offset: 0x00004C0C
		private List<double[]> AE859F07(List<double[]> BFA5BA3D, 469F9E21.14211E00 89ADA53A, List<double[]> CD10B8BF, double 058501A8)
		{
			List<double[]> list = new List<double[]>();
			List<double[,]> list2 = new List<double[,]>();
			List<double[]> list3 = new List<double[]>();
			for (int i = 0; i < BFA5BA3D.Count; i++)
			{
				89ADA53A.7B132382(CD10B8BF[i], true);
				ValueTuple<double[], double[,], double[]> valueTuple = 89ADA53A.F6B0FEB1(BFA5BA3D[i]);
				double[] item = valueTuple.Item1;
				double[,] item2 = valueTuple.Item2;
				double[] item3 = valueTuple.Item3;
				list.Add(item);
				list2.Add(item2);
				list3.Add(item3);
			}
			bool flag;
			if (058501A8 > 0.0)
			{
				flag = list2.Any(new Func<double[,], bool>(469F9E21.78B3B48A.E00D94AC.<>9.1024A99F));
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				double[,] array = this.85BA0CA1(list2.Where(new Func<double[,], bool>(469F9E21.78B3B48A.E00D94AC.<>9.C50EA52A)).ToList<double[,]>());
				double[] array2 = this.2996BC9E(list3.Where(new Func<double[], bool>(469F9E21.78B3B48A.E00D94AC.<>9.D4202023)).ToList<double[]>());
				bool flag3 = array != null && array2 != null;
				if (flag3)
				{
					89ADA53A.208A2188(array, array2, 058501A8);
				}
			}
			return list;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006960 File Offset: 0x00004D60
		private double[,] 85BA0CA1(List<double[,]> D7092C80)
		{
			bool flag;
			if (D7092C80 != null && D7092C80.Count != 0)
			{
				flag = D7092C80.All(new Func<double[,], bool>(469F9E21.78B3B48A.E00D94AC.<>9.CB8A8389));
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			double[,] result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				D7092C80 = D7092C80.Where(new Func<double[,], bool>(469F9E21.78B3B48A.E00D94AC.<>9.DE37C4B8)).ToList<double[,]>();
				bool flag3 = D7092C80.Count == 0;
				if (flag3)
				{
					result = null;
				}
				else
				{
					int length = D7092C80[0].GetLength(0);
					int length2 = D7092C80[0].GetLength(1);
					double[,] array = new double[length, length2];
					foreach (double[,] array2 in D7092C80)
					{
						for (int i = 0; i < length; i++)
						{
							for (int j = 0; j < length2; j++)
							{
								array[i, j] += array2[i, j];
							}
						}
					}
					for (int k = 0; k < length; k++)
					{
						for (int l = 0; l < length2; l++)
						{
							array[k, l] /= (double)D7092C80.Count;
						}
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006AE0 File Offset: 0x00004EE0
		private double[] 2996BC9E(List<double[]> E7BB522E)
		{
			bool flag;
			if (E7BB522E != null && E7BB522E.Count != 0)
			{
				flag = E7BB522E.All(new Func<double[], bool>(469F9E21.78B3B48A.E00D94AC.<>9.F6891D1F));
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			double[] result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				E7BB522E = E7BB522E.Where(new Func<double[], bool>(469F9E21.78B3B48A.E00D94AC.<>9.1E9DFFB0)).ToList<double[]>();
				bool flag3 = E7BB522E.Count == 0;
				if (flag3)
				{
					result = null;
				}
				else
				{
					int num = E7BB522E[0].Length;
					double[] array = new double[num];
					foreach (double[] array2 in E7BB522E)
					{
						for (int i = 0; i < num; i++)
						{
							array[i] += array2[i];
						}
					}
					for (int j = 0; j < num; j++)
					{
						array[j] /= (double)E7BB522E.Count;
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006C14 File Offset: 0x00005014
		public object 00B4C524()
		{
			return new 1D187216<object[], object[], object[], object>(this.B03A8F31.Select(new Func<469F9E21.14211E00, object>(469F9E21.78B3B48A.E00D94AC.<>9.80B92B13)).ToArray<object>(), this.9F973697.Select(new Func<469F9E21.14211E00, object>(469F9E21.78B3B48A.E00D94AC.<>9.FD28E58C)).ToArray<object>(), this.28AE4502.Select(new Func<469F9E21.14211E00, object>(469F9E21.78B3B48A.E00D94AC.<>9.E41F7C88)).ToArray<object>(), this.21809C32.3A811634());
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00006CC0 File Offset: 0x000050C0
		public void 82AFFA06(JsonElement FE34AD84)
		{
			List<JsonElement> list = FE34AD84.GetProperty("WqLayers").EnumerateArray().ToList<JsonElement>();
			List<JsonElement> list2 = FE34AD84.GetProperty("WkLayers").EnumerateArray().ToList<JsonElement>();
			List<JsonElement> list3 = FE34AD84.GetProperty("WvLayers").EnumerateArray().ToList<JsonElement>();
			for (int i = 0; i < this.288DC303; i++)
			{
				this.B03A8F31[i].4486B5B7(list[i]);
				this.9F973697[i].4486B5B7(list2[i]);
				this.28AE4502[i].4486B5B7(list3[i]);
			}
			this.21809C32.4486B5B7(FE34AD84.GetProperty("WoLayer"));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00006DB1 File Offset: 0x000051B1
		[CompilerGenerated]
		private double[] D91D208D(int 9D057E8C)
		{
			return new double[this.24884523];
		}

		// Token: 0x04000059 RID: 89
		private readonly int 24884523;

		// Token: 0x0400005A RID: 90
		private readonly int 288DC303;

		// Token: 0x0400005B RID: 91
		private readonly int 892E4FA9;

		// Token: 0x0400005C RID: 92
		private readonly int 6305A81C;

		// Token: 0x0400005D RID: 93
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<469F9E21.14211E00> 6A3982B6;

		// Token: 0x0400005E RID: 94
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<469F9E21.14211E00> 30BA39B9;

		// Token: 0x0400005F RID: 95
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<469F9E21.14211E00> 2D8F331E;

		// Token: 0x04000060 RID: 96
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private 469F9E21.14211E00 77010D80;

		// Token: 0x04000061 RID: 97
		private List<double[]> BFA3B628;

		// Token: 0x04000062 RID: 98
		private List<List<double[]>> D133A931;

		// Token: 0x04000063 RID: 99
		private List<List<double[]>> 53B3C70D;

		// Token: 0x04000064 RID: 100
		private List<List<double[]>> 5B04398F;

		// Token: 0x04000065 RID: 101
		private List<double[,]> C60F14BA;

		// Token: 0x04000066 RID: 102
		private List<double[]> 2AAC70B9;

		// Token: 0x02000022 RID: 34
		[CompilerGenerated]
		[Serializable]
		private sealed class E00D94AC
		{
			// Token: 0x060000DB RID: 219 RVA: 0x00008958 File Offset: 0x00006D58
			internal double[] E917C49A(double[] A0B98B2E)
			{
				return (double[])A0B98B2E.Clone();
			}

			// Token: 0x060000DC RID: 220 RVA: 0x00008965 File Offset: 0x00006D65
			internal double[] 2D934B8D(double[] 991A53A9)
			{
				return (double[])991A53A9.Clone();
			}

			// Token: 0x060000DD RID: 221 RVA: 0x00008972 File Offset: 0x00006D72
			internal bool 3E9D04A1(double[,] FA0B0685)
			{
				return FA0B0685 != null;
			}

			// Token: 0x060000DE RID: 222 RVA: 0x00008978 File Offset: 0x00006D78
			internal bool 0A14AC08(double[,] 69308593)
			{
				return 69308593 != null;
			}

			// Token: 0x060000DF RID: 223 RVA: 0x0000897E File Offset: 0x00006D7E
			internal bool 433B13B8(double[] DE0503BB)
			{
				return DE0503BB != null;
			}

			// Token: 0x060000E0 RID: 224 RVA: 0x00008984 File Offset: 0x00006D84
			internal bool 1024A99F(double[,] 4584E50F)
			{
				return 4584E50F != null;
			}

			// Token: 0x060000E1 RID: 225 RVA: 0x0000898A File Offset: 0x00006D8A
			internal bool C50EA52A(double[,] AA151C34)
			{
				return AA151C34 != null;
			}

			// Token: 0x060000E2 RID: 226 RVA: 0x00008990 File Offset: 0x00006D90
			internal bool D4202023(double[] 4002E431)
			{
				return 4002E431 != null;
			}

			// Token: 0x060000E3 RID: 227 RVA: 0x00008996 File Offset: 0x00006D96
			internal bool CB8A8389(double[,] 429BE598)
			{
				return 429BE598 == null;
			}

			// Token: 0x060000E4 RID: 228 RVA: 0x0000899C File Offset: 0x00006D9C
			internal bool DE37C4B8(double[,] 36B5DBAE)
			{
				return 36B5DBAE != null;
			}

			// Token: 0x060000E5 RID: 229 RVA: 0x000089A2 File Offset: 0x00006DA2
			internal bool F6891D1F(double[] C8160DBE)
			{
				return C8160DBE == null;
			}

			// Token: 0x060000E6 RID: 230 RVA: 0x000089A8 File Offset: 0x00006DA8
			internal bool 1E9DFFB0(double[] 170AF3BE)
			{
				return 170AF3BE != null;
			}

			// Token: 0x060000E7 RID: 231 RVA: 0x000089AE File Offset: 0x00006DAE
			internal object 80B92B13(469F9E21.14211E00 E63D92B4)
			{
				return E63D92B4.3A811634();
			}

			// Token: 0x060000E8 RID: 232 RVA: 0x000089B6 File Offset: 0x00006DB6
			internal object FD28E58C(469F9E21.14211E00 C08C2F92)
			{
				return C08C2F92.3A811634();
			}

			// Token: 0x060000E9 RID: 233 RVA: 0x000089BE File Offset: 0x00006DBE
			internal object E41F7C88(469F9E21.14211E00 C82D1F39)
			{
				return C82D1F39.3A811634();
			}

			// Token: 0x04000097 RID: 151
			public static readonly 469F9E21.78B3B48A.E00D94AC <>9 = new 469F9E21.78B3B48A.E00D94AC();

			// Token: 0x04000098 RID: 152
			public static Func<double[], double[]> <>9__27_0;

			// Token: 0x04000099 RID: 153
			public static Func<double[], double[]> <>9__27_1;

			// Token: 0x0400009A RID: 154
			public static Func<double[,], bool> <>9__28_1;

			// Token: 0x0400009B RID: 155
			public static Func<double[,], bool> <>9__28_2;

			// Token: 0x0400009C RID: 156
			public static Func<double[], bool> <>9__28_3;

			// Token: 0x0400009D RID: 157
			public static Func<double[,], bool> <>9__29_0;

			// Token: 0x0400009E RID: 158
			public static Func<double[,], bool> <>9__29_1;

			// Token: 0x0400009F RID: 159
			public static Func<double[], bool> <>9__29_2;

			// Token: 0x040000A0 RID: 160
			public static Func<double[,], bool> <>9__30_1;

			// Token: 0x040000A1 RID: 161
			public static Func<double[,], bool> <>9__30_0;

			// Token: 0x040000A2 RID: 162
			public static Func<double[], bool> <>9__31_1;

			// Token: 0x040000A3 RID: 163
			public static Func<double[], bool> <>9__31_0;

			// Token: 0x040000A4 RID: 164
			public static Func<469F9E21.14211E00, object> <>9__32_0;

			// Token: 0x040000A5 RID: 165
			public static Func<469F9E21.14211E00, object> <>9__32_1;

			// Token: 0x040000A6 RID: 166
			public static Func<469F9E21.14211E00, object> <>9__32_2;
		}

		// Token: 0x02000023 RID: 35
		[CompilerGenerated]
		private sealed class EB86542F
		{
			// Token: 0x060000EB RID: 235 RVA: 0x000089CF File Offset: 0x00006DCF
			internal 469F9E21.14211E00 0CB2538C(int 862E9615)
			{
				return new 469F9E21.14211E00(this.CF9B6899, this.15BA9327.892E4FA9, "ActivationIdentity");
			}

			// Token: 0x060000EC RID: 236 RVA: 0x000089EC File Offset: 0x00006DEC
			internal 469F9E21.14211E00 1887E238(int A9AF80B8)
			{
				return new 469F9E21.14211E00(this.CF9B6899, this.15BA9327.892E4FA9, "ActivationIdentity");
			}

			// Token: 0x060000ED RID: 237 RVA: 0x00008A09 File Offset: 0x00006E09
			internal 469F9E21.14211E00 5CA02732(int 512D7CBD)
			{
				return new 469F9E21.14211E00(this.CF9B6899, this.15BA9327.6305A81C, "ActivationIdentity");
			}

			// Token: 0x040000A7 RID: 167
			public int CF9B6899;

			// Token: 0x040000A8 RID: 168
			public 469F9E21.78B3B48A 15BA9327;
		}

		// Token: 0x02000024 RID: 36
		[CompilerGenerated]
		private sealed class 071D6092
		{
			// Token: 0x060000EF RID: 239 RVA: 0x00008A2F File Offset: 0x00006E2F
			internal double[] 7A1F3486(double[] 09054620)
			{
				return this.5AAD01B1.21809C32.7B132382(09054620, this.60989102);
			}

			// Token: 0x040000A9 RID: 169
			public 469F9E21.78B3B48A 5AAD01B1;

			// Token: 0x040000AA RID: 170
			public bool 60989102;
		}
	}

	// Token: 0x02000017 RID: 23
	public class 82B3E131
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00006DBE File Offset: 0x000051BE
		public double 9F2549A9 { get; }

		// Token: 0x0600009F RID: 159 RVA: 0x00006DC8 File Offset: 0x000051C8
		public 82B3E131(int E9267C0F, int 56805A13, double 290E0A81 = 0.0)
		{
			this.9F2549A9 = 290E0A81;
			this.A00DE91E = new 469F9E21.14211E00(E9267C0F, 56805A13, "ActivationRamp");
			this.5C207C3C = new 469F9E21.14211E00(56805A13, E9267C0F, "ActivationIdentity");
			bool flag = 290E0A81 > 0.0;
			if (flag)
			{
				this.B72BF5BC = new 469F9E21.8482B4BA(290E0A81);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006E24 File Offset: 0x00005224
		public List<double[]> 0724EE07(List<double[]> 7E237939, bool 3A10A72F = false)
		{
			469F9E21.82B3E131.7733CB8B 7733CB8B = new 469F9E21.82B3E131.7733CB8B();
			7733CB8B.3C11F09A = this;
			7733CB8B.EFA281B3 = 3A10A72F;
			this.71A23A09 = 7E237939.Select(new Func<double[], double[]>(469F9E21.82B3E131.FF29DB9A.<>9.6A9E9806)).ToList<double[]>();
			List<double[]> source = 7E237939.Select(new Func<double[], double[]>(7733CB8B.6D00DA90)).ToList<double[]>();
			this.521FF898 = source;
			bool flag = this.B72BF5BC != null & 7733CB8B.EFA281B3;
			if (flag)
			{
				this.521FF898 = source.Select(new Func<double[], double[]>(7733CB8B.58B5CE9C)).ToList<double[]>();
			}
			return this.521FF898.Select(new Func<double[], double[]>(7733CB8B.140BC63F)).ToList<double[]>();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006EEC File Offset: 0x000052EC
		public List<double[]> 8B93A4BF(List<double[]> C4382E1C, double 27884713)
		{
			List<double[]> list = new List<double[]>();
			List<double[,]> list2 = new List<double[,]>();
			List<double[]> list3 = new List<double[]>();
			for (int i = 0; i < C4382E1C.Count; i++)
			{
				this.5C207C3C.7B132382(this.521FF898[i], true);
				ValueTuple<double[], double[,], double[]> valueTuple = this.5C207C3C.F6B0FEB1(C4382E1C[i]);
				double[] item = valueTuple.Item1;
				double[,] item2 = valueTuple.Item2;
				double[] item3 = valueTuple.Item3;
				list.Add(item);
				list2.Add(item2);
				list3.Add(item3);
			}
			bool flag;
			if (27884713 > 0.0)
			{
				flag = list2.Any(new Func<double[,], bool>(469F9E21.82B3E131.FF29DB9A.<>9.45108297));
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				double[,] array = this.6AA730AC(list2.Where(new Func<double[,], bool>(469F9E21.82B3E131.FF29DB9A.<>9.B5B6B5B4)).ToList<double[,]>());
				double[] array2 = this.750D9807(list3.Where(new Func<double[], bool>(469F9E21.82B3E131.FF29DB9A.<>9.BE1F58A4)).ToList<double[]>());
				bool flag3 = array != null && array2 != null;
				if (flag3)
				{
					this.5C207C3C.208A2188(array, array2, 27884713);
				}
			}
			List<double[]> list4 = list;
			bool flag4 = this.B72BF5BC != null;
			if (flag4)
			{
				list4 = list.Select(new Func<double[], double[]>(this.93B4340A)).ToList<double[]>();
			}
			List<double[]> list5 = new List<double[]>();
			List<double[,]> list6 = new List<double[,]>();
			List<double[]> list7 = new List<double[]>();
			for (int j = 0; j < list4.Count; j++)
			{
				this.A00DE91E.7B132382(this.71A23A09[j], true);
				ValueTuple<double[], double[,], double[]> valueTuple2 = this.A00DE91E.F6B0FEB1(list4[j]);
				double[] item4 = valueTuple2.Item1;
				double[,] item5 = valueTuple2.Item2;
				double[] item6 = valueTuple2.Item3;
				list5.Add(item4);
				list6.Add(item5);
				list7.Add(item6);
			}
			bool flag5;
			if (27884713 > 0.0)
			{
				flag5 = list6.Any(new Func<double[,], bool>(469F9E21.82B3E131.FF29DB9A.<>9.EF0AFB81));
			}
			else
			{
				flag5 = false;
			}
			bool flag6 = flag5;
			if (flag6)
			{
				double[,] array3 = this.6AA730AC(list6.Where(new Func<double[,], bool>(469F9E21.82B3E131.FF29DB9A.<>9.F0992993)).ToList<double[,]>());
				double[] array4 = this.750D9807(list7.Where(new Func<double[], bool>(469F9E21.82B3E131.FF29DB9A.<>9.CD24100C)).ToList<double[]>());
				bool flag7 = array3 != null && array4 != null;
				if (flag7)
				{
					this.A00DE91E.208A2188(array3, array4, 27884713);
				}
			}
			return list5;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000071DC File Offset: 0x000055DC
		private double[,] 6AA730AC(List<double[,]> D41EA29A)
		{
			bool flag;
			if (D41EA29A != null && D41EA29A.Count != 0)
			{
				flag = D41EA29A.All(new Func<double[,], bool>(469F9E21.82B3E131.FF29DB9A.<>9.1117DD95));
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			double[,] result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				D41EA29A = D41EA29A.Where(new Func<double[,], bool>(469F9E21.82B3E131.FF29DB9A.<>9.7B179D01)).ToList<double[,]>();
				bool flag3 = D41EA29A.Count == 0;
				if (flag3)
				{
					result = null;
				}
				else
				{
					int length = D41EA29A[0].GetLength(0);
					int length2 = D41EA29A[0].GetLength(1);
					double[,] array = new double[length, length2];
					foreach (double[,] array2 in D41EA29A)
					{
						for (int i = 0; i < length; i++)
						{
							for (int j = 0; j < length2; j++)
							{
								array[i, j] += array2[i, j];
							}
						}
					}
					for (int k = 0; k < length; k++)
					{
						for (int l = 0; l < length2; l++)
						{
							array[k, l] /= (double)D41EA29A.Count;
						}
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000735C File Offset: 0x0000575C
		private double[] 750D9807(List<double[]> CA016E36)
		{
			bool flag;
			if (CA016E36 != null && CA016E36.Count != 0)
			{
				flag = CA016E36.All(new Func<double[], bool>(469F9E21.82B3E131.FF29DB9A.<>9.80AF7582));
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			double[] result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				CA016E36 = CA016E36.Where(new Func<double[], bool>(469F9E21.82B3E131.FF29DB9A.<>9.AF334800)).ToList<double[]>();
				bool flag3 = CA016E36.Count == 0;
				if (flag3)
				{
					result = null;
				}
				else
				{
					int num = CA016E36[0].Length;
					double[] array = new double[num];
					foreach (double[] array2 in CA016E36)
					{
						for (int i = 0; i < num; i++)
						{
							array[i] += array2[i];
						}
					}
					for (int j = 0; j < num; j++)
					{
						array[j] /= (double)CA016E36.Count;
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007490 File Offset: 0x00005890
		public object D091C6BA()
		{
			return new D3065C30<object, object, double>(this.A00DE91E.3A811634(), this.5C207C3C.3A811634(), this.9F2549A9);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000074B3 File Offset: 0x000058B3
		public void 36388E82(JsonElement A70B1081)
		{
			this.A00DE91E.4486B5B7(A70B1081.GetProperty("layer1"));
			this.5C207C3C.4486B5B7(A70B1081.GetProperty("layer2"));
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000074E6 File Offset: 0x000058E6
		[CompilerGenerated]
		private double[] 93B4340A(double[] ADB23C37)
		{
			return this.B72BF5BC.721B1D97(ADB23C37, 0.0);
		}

		// Token: 0x04000067 RID: 103
		private readonly 469F9E21.14211E00 A00DE91E;

		// Token: 0x04000068 RID: 104
		private readonly 469F9E21.14211E00 5C207C3C;

		// Token: 0x04000069 RID: 105
		private readonly 469F9E21.8482B4BA B72BF5BC;

		// Token: 0x0400006A RID: 106
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly double C8102237;

		// Token: 0x0400006B RID: 107
		private List<double[]> 71A23A09;

		// Token: 0x0400006C RID: 108
		private List<double[]> 521FF898;

		// Token: 0x02000025 RID: 37
		[CompilerGenerated]
		[Serializable]
		private sealed class FF29DB9A
		{
			// Token: 0x060000F2 RID: 242 RVA: 0x00008A5D File Offset: 0x00006E5D
			internal double[] 6A9E9806(double[] 610CF02F)
			{
				return (double[])610CF02F.Clone();
			}

			// Token: 0x060000F3 RID: 243 RVA: 0x00008A6A File Offset: 0x00006E6A
			internal bool 45108297(double[,] 798A7618)
			{
				return 798A7618 != null;
			}

			// Token: 0x060000F4 RID: 244 RVA: 0x00008A70 File Offset: 0x00006E70
			internal bool B5B6B5B4(double[,] B03D403C)
			{
				return B03D403C != null;
			}

			// Token: 0x060000F5 RID: 245 RVA: 0x00008A76 File Offset: 0x00006E76
			internal bool BE1F58A4(double[] 13866ABD)
			{
				return 13866ABD != null;
			}

			// Token: 0x060000F6 RID: 246 RVA: 0x00008A7C File Offset: 0x00006E7C
			internal bool EF0AFB81(double[,] F439A82D)
			{
				return F439A82D != null;
			}

			// Token: 0x060000F7 RID: 247 RVA: 0x00008A82 File Offset: 0x00006E82
			internal bool F0992993(double[,] 8A0FCB17)
			{
				return 8A0FCB17 != null;
			}

			// Token: 0x060000F8 RID: 248 RVA: 0x00008A88 File Offset: 0x00006E88
			internal bool CD24100C(double[] 409C1930)
			{
				return 409C1930 != null;
			}

			// Token: 0x060000F9 RID: 249 RVA: 0x00008A8E File Offset: 0x00006E8E
			internal bool 1117DD95(double[,] DA0EA681)
			{
				return DA0EA681 == null;
			}

			// Token: 0x060000FA RID: 250 RVA: 0x00008A94 File Offset: 0x00006E94
			internal bool 7B179D01(double[,] 47802614)
			{
				return 47802614 != null;
			}

			// Token: 0x060000FB RID: 251 RVA: 0x00008A9A File Offset: 0x00006E9A
			internal bool 80AF7582(double[] 9C06ABAF)
			{
				return 9C06ABAF == null;
			}

			// Token: 0x060000FC RID: 252 RVA: 0x00008AA0 File Offset: 0x00006EA0
			internal bool AF334800(double[] 3DAF09B2)
			{
				return 3DAF09B2 != null;
			}

			// Token: 0x040000AB RID: 171
			public static readonly 469F9E21.82B3E131.FF29DB9A <>9 = new 469F9E21.82B3E131.FF29DB9A();

			// Token: 0x040000AC RID: 172
			public static Func<double[], double[]> <>9__9_0;

			// Token: 0x040000AD RID: 173
			public static Func<double[,], bool> <>9__10_1;

			// Token: 0x040000AE RID: 174
			public static Func<double[,], bool> <>9__10_2;

			// Token: 0x040000AF RID: 175
			public static Func<double[], bool> <>9__10_3;

			// Token: 0x040000B0 RID: 176
			public static Func<double[,], bool> <>9__10_4;

			// Token: 0x040000B1 RID: 177
			public static Func<double[,], bool> <>9__10_5;

			// Token: 0x040000B2 RID: 178
			public static Func<double[], bool> <>9__10_6;

			// Token: 0x040000B3 RID: 179
			public static Func<double[,], bool> <>9__11_1;

			// Token: 0x040000B4 RID: 180
			public static Func<double[,], bool> <>9__11_0;

			// Token: 0x040000B5 RID: 181
			public static Func<double[], bool> <>9__12_1;

			// Token: 0x040000B6 RID: 182
			public static Func<double[], bool> <>9__12_0;
		}

		// Token: 0x02000026 RID: 38
		[CompilerGenerated]
		private sealed class 7733CB8B
		{
			// Token: 0x060000FE RID: 254 RVA: 0x00008AAF File Offset: 0x00006EAF
			internal double[] 6D00DA90(double[] F9338580)
			{
				return this.3C11F09A.A00DE91E.7B132382(F9338580, this.EFA281B3);
			}

			// Token: 0x060000FF RID: 255 RVA: 0x00008AC8 File Offset: 0x00006EC8
			internal double[] 58B5CE9C(double[] 5B2ED424)
			{
				return this.3C11F09A.B72BF5BC.7B132382(5B2ED424, this.EFA281B3);
			}

			// Token: 0x06000100 RID: 256 RVA: 0x00008AE1 File Offset: 0x00006EE1
			internal double[] 140BC63F(double[] 38A15421)
			{
				return this.3C11F09A.5C207C3C.7B132382(38A15421, this.EFA281B3);
			}

			// Token: 0x040000B7 RID: 183
			public 469F9E21.82B3E131 3C11F09A;

			// Token: 0x040000B8 RID: 184
			public bool EFA281B3;
		}
	}

	// Token: 0x02000018 RID: 24
	public class 5EA225AE
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000074FD File Offset: 0x000058FD
		public double EB844D99 { get; }

		// Token: 0x060000A8 RID: 168 RVA: 0x00007508 File Offset: 0x00005908
		public 5EA225AE(int 73851EB9, int 91049390, int 4113572A, double 892D65BB = 0.1)
		{
			this.EB844D99 = 892D65BB;
			this.A6BA1218 = new 469F9E21.78B3B48A(73851EB9, 91049390);
			this.DDA4C18E = new 469F9E21.82B3E131(73851EB9, 4113572A, 892D65BB);
			this.21B35C13 = new 469F9E21.30907DB3(73851EB9);
			this.DE0AE401 = new 469F9E21.30907DB3(73851EB9);
			bool flag = 892D65BB > 0.0;
			if (flag)
			{
				this.C337C604 = new 469F9E21.8482B4BA(892D65BB);
				this.8A34051C = new 469F9E21.8482B4BA(892D65BB);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007588 File Offset: 0x00005988
		public List<double[]> DD0D8E8D(List<double[]> 1A9CC43F, bool C2AB1B1E = false)
		{
			469F9E21.5EA225AE.F5BF0B01 f5BF0B = new 469F9E21.5EA225AE.F5BF0B01();
			f5BF0B.8137BE3F = this;
			f5BF0B.0D9C4481 = C2AB1B1E;
			List<double[]> list = this.A6BA1218.C83DD887(1A9CC43F, f5BF0B.0D9C4481);
			List<double[]> 17BB = list;
			bool flag = this.C337C604 != null & f5BF0B.0D9C4481;
			if (flag)
			{
				17BB = list.Select(new Func<double[], double[]>(f5BF0B.FD3CEBAF)).ToList<double[]>();
			}
			List<double[]> source = 469F9E21.C3259C27.DE1DDC21(1A9CC43F, 17BB);
			List<double[]> list2 = source.Select(new Func<double[], double[]>(f5BF0B.A8B8C617)).ToList<double[]>();
			List<double[]> list3 = this.DDA4C18E.0724EE07(list2, f5BF0B.0D9C4481);
			List<double[]> 17BB2 = list3;
			bool flag2 = this.8A34051C != null & f5BF0B.0D9C4481;
			if (flag2)
			{
				17BB2 = list3.Select(new Func<double[], double[]>(f5BF0B.C511A338)).ToList<double[]>();
			}
			List<double[]> source2 = 469F9E21.C3259C27.DE1DDC21(list2, 17BB2);
			return source2.Select(new Func<double[], double[]>(f5BF0B.E3194096)).ToList<double[]>();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007684 File Offset: 0x00005A84
		public List<double[]> 7AA37BAF(List<double[]> DE2AA808, double E3252015)
		{
			int count = DE2AA808.Count;
			List<double[]> list = new List<double[]>(count);
			for (int i = 0; i < count; i++)
			{
				list.Add(this.DE0AE401.721B1D97(DE2AA808[i], E3252015));
			}
			List<double[]> e5230E = list.Select(new Func<double[], double[]>(469F9E21.5EA225AE.AE0A1C0C.<>9.F4003FAB)).ToList<double[]>();
			List<double[]> list2 = list.Select(new Func<double[], double[]>(469F9E21.5EA225AE.AE0A1C0C.<>9.A539CA21)).ToList<double[]>();
			List<double[]> c4382E1C = list2;
			bool flag = this.8A34051C != null;
			if (flag)
			{
				c4382E1C = list2.Select(new Func<double[], double[]>(this.A48C8930)).ToList<double[]>();
			}
			List<double[]> 17BB = this.DDA4C18E.8B93A4BF(c4382E1C, E3252015);
			List<double[]> list3 = 469F9E21.C3259C27.DE1DDC21(e5230E, 17BB);
			List<double[]> list4 = new List<double[]>(count);
			for (int j = 0; j < count; j++)
			{
				list4.Add(this.21B35C13.721B1D97(list3[j], E3252015));
			}
			List<double[]> e5230E2 = list4.Select(new Func<double[], double[]>(469F9E21.5EA225AE.AE0A1C0C.<>9.7C89541A)).ToList<double[]>();
			List<double[]> list5 = list4.Select(new Func<double[], double[]>(469F9E21.5EA225AE.AE0A1C0C.<>9.C43C7DAB)).ToList<double[]>();
			List<double[]> 55B2D6A = list5;
			bool flag2 = this.C337C604 != null;
			if (flag2)
			{
				55B2D6A = list5.Select(new Func<double[], double[]>(this.1A9B402A)).ToList<double[]>();
			}
			List<double[]> 17BB2 = this.A6BA1218.F8A5CDA6(55B2D6A, E3252015);
			return 469F9E21.C3259C27.DE1DDC21(e5230E2, 17BB2);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007851 File Offset: 0x00005C51
		public object 889384A3()
		{
			return new E73AEAAE<object, object, object, object, double>(this.A6BA1218.00B4C524(), this.DDA4C18E.D091C6BA(), this.21B35C13.3A811634(), this.DE0AE401.3A811634(), this.EB844D99);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000788C File Offset: 0x00005C8C
		public void CDA2720E(JsonElement B21459A2)
		{
			this.A6BA1218.82AFFA06(B21459A2.GetProperty("mha"));
			this.DDA4C18E.36388E82(B21459A2.GetProperty("ffn"));
			this.21B35C13.4486B5B7(B21459A2.GetProperty("norm1"));
			this.DE0AE401.4486B5B7(B21459A2.GetProperty("norm2"));
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000078FA File Offset: 0x00005CFA
		[CompilerGenerated]
		private double[] A48C8930(double[] 0B868B8A)
		{
			return this.8A34051C.721B1D97(0B868B8A, 0.0);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00007911 File Offset: 0x00005D11
		[CompilerGenerated]
		private double[] 1A9B402A(double[] 143F3B9A)
		{
			return this.C337C604.721B1D97(143F3B9A, 0.0);
		}

		// Token: 0x0400006D RID: 109
		private readonly 469F9E21.78B3B48A A6BA1218;

		// Token: 0x0400006E RID: 110
		private readonly 469F9E21.82B3E131 DDA4C18E;

		// Token: 0x0400006F RID: 111
		private readonly 469F9E21.30907DB3 21B35C13;

		// Token: 0x04000070 RID: 112
		private readonly 469F9E21.30907DB3 DE0AE401;

		// Token: 0x04000071 RID: 113
		private readonly 469F9E21.8482B4BA C337C604;

		// Token: 0x04000072 RID: 114
		private readonly 469F9E21.8482B4BA 8A34051C;

		// Token: 0x04000073 RID: 115
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly double 43B53435;

		// Token: 0x02000027 RID: 39
		[CompilerGenerated]
		[Serializable]
		private sealed class AE0A1C0C
		{
			// Token: 0x06000103 RID: 259 RVA: 0x00008B0F File Offset: 0x00006F0F
			internal double[] F4003FAB(double[] FDAEB429)
			{
				return (double[])FDAEB429.Clone();
			}

			// Token: 0x06000104 RID: 260 RVA: 0x00008B1C File Offset: 0x00006F1C
			internal double[] A539CA21(double[] F70B4592)
			{
				return (double[])F70B4592.Clone();
			}

			// Token: 0x06000105 RID: 261 RVA: 0x00008B29 File Offset: 0x00006F29
			internal double[] 7C89541A(double[] 6AAA5FA5)
			{
				return (double[])6AAA5FA5.Clone();
			}

			// Token: 0x06000106 RID: 262 RVA: 0x00008B36 File Offset: 0x00006F36
			internal double[] C43C7DAB(double[] F3AA532B)
			{
				return (double[])F3AA532B.Clone();
			}

			// Token: 0x040000B9 RID: 185
			public static readonly 469F9E21.5EA225AE.AE0A1C0C <>9 = new 469F9E21.5EA225AE.AE0A1C0C();

			// Token: 0x040000BA RID: 186
			public static Func<double[], double[]> <>9__11_0;

			// Token: 0x040000BB RID: 187
			public static Func<double[], double[]> <>9__11_1;

			// Token: 0x040000BC RID: 188
			public static Func<double[], double[]> <>9__11_3;

			// Token: 0x040000BD RID: 189
			public static Func<double[], double[]> <>9__11_4;
		}

		// Token: 0x02000028 RID: 40
		[CompilerGenerated]
		private sealed class F5BF0B01
		{
			// Token: 0x06000108 RID: 264 RVA: 0x00008B4C File Offset: 0x00006F4C
			internal double[] FD3CEBAF(double[] 313E9E06)
			{
				return this.8137BE3F.C337C604.7B132382(313E9E06, this.0D9C4481);
			}

			// Token: 0x06000109 RID: 265 RVA: 0x00008B65 File Offset: 0x00006F65
			internal double[] A8B8C617(double[] 84BE16A2)
			{
				return this.8137BE3F.21B35C13.7B132382(84BE16A2, this.0D9C4481);
			}

			// Token: 0x0600010A RID: 266 RVA: 0x00008B7E File Offset: 0x00006F7E
			internal double[] C511A338(double[] 2B08F592)
			{
				return this.8137BE3F.8A34051C.7B132382(2B08F592, this.0D9C4481);
			}

			// Token: 0x0600010B RID: 267 RVA: 0x00008B97 File Offset: 0x00006F97
			internal double[] E3194096(double[] A9B9C92E)
			{
				return this.8137BE3F.DE0AE401.7B132382(A9B9C92E, this.0D9C4481);
			}

			// Token: 0x040000BE RID: 190
			public 469F9E21.5EA225AE 8137BE3F;

			// Token: 0x040000BF RID: 191
			public bool 0D9C4481;
		}
	}

	// Token: 0x02000019 RID: 25
	public class F43F251C : 469F9E21.FD81572E
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00007928 File Offset: 0x00005D28
		public double C031EFA1 { get; }

		// Token: 0x060000B0 RID: 176 RVA: 0x00007930 File Offset: 0x00005D30
		public F43F251C(int 7C8DEBAA, int 8CBCD9BC, int D3AC2DB7, int 01B11D8D, int F22CA933, int A68AAF82, double 7B004AA8 = 0.1)
		{
			469F9E21.F43F251C.0BAE3EAC 0BAE3EAC = new 469F9E21.F43F251C.0BAE3EAC();
			0BAE3EAC.1B3BD2A1 = D3AC2DB7;
			0BAE3EAC.FC1D90BB = 01B11D8D;
			0BAE3EAC.AEBB2F0C = A68AAF82;
			0BAE3EAC.57AF7F13 = 7B004AA8;
			base..ctor();
			this.C031EFA1 = 0BAE3EAC.57AF7F13;
			this.AC34C43D = 7C8DEBAA;
			this.94994E1D = 8CBCD9BC;
			this.15BC9428 = 0BAE3EAC.1B3BD2A1;
			this.E38827AB = new 469F9E21.14211E00(8CBCD9BC, 0BAE3EAC.1B3BD2A1, "ActivationIdentity");
			this.B191B336 = new 469F9E21.0C9957BA(0BAE3EAC.1B3BD2A1, Math.Max(7C8DEBAA, 100));
			bool flag = 0BAE3EAC.57AF7F13 > 0.0;
			if (flag)
			{
				this.33AB4836 = new 469F9E21.8482B4BA(0BAE3EAC.57AF7F13);
			}
			this.3C880A28 = Enumerable.Range(0, F22CA933).Select(new Func<int, 469F9E21.5EA225AE>(0BAE3EAC.989E090A)).ToList<469F9E21.5EA225AE>();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00007A0C File Offset: 0x00005E0C
		public override double[] 7B132382(double[] 90B76A93, bool F9A5DC15 = false)
		{
			469F9E21.F43F251C.E213D49A e213D49A = new 469F9E21.F43F251C.E213D49A();
			e213D49A.06AC4738 = this;
			e213D49A.17853590 = F9A5DC15;
			bool flag = 90B76A93.Length != this.AC34C43D * this.94994E1D;
			if (flag)
			{
				throw new ArgumentException(string.Format("TransformerModel: Girdi boyutu {0} != beklenen {1}.", 90B76A93.Length, this.AC34C43D * this.94994E1D));
			}
			this.6D939AA6 = new List<double[]>();
			List<double[]> list = new List<double[]>();
			for (int i = 0; i < this.AC34C43D; i++)
			{
				double[] array = new double[this.94994E1D];
				Array.Copy(90B76A93, i * this.94994E1D, array, 0, this.94994E1D);
				this.6D939AA6.Add(array);
				list.Add(this.E38827AB.7B132382(array, e213D49A.17853590));
			}
			this.B7A9BB3C = list;
			bool flag2 = this.33AB4836 != null & e213D49A.17853590;
			if (flag2)
			{
				this.B7A9BB3C = list.Select(new Func<double[], double[]>(e213D49A.850AD8A8)).ToList<double[]>();
			}
			List<double[]> list2 = this.B191B336.4A1BEA80(this.B7A9BB3C);
			List<double[]> list3 = list2;
			foreach (469F9E21.5EA225AE 5EA225AE in this.3C880A28)
			{
				list3 = 5EA225AE.DD0D8E8D(list3, e213D49A.17853590);
			}
			return (double[])list3.Last<double[]>().Clone();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00007BA8 File Offset: 0x00005FA8
		public override double[] 721B1D97(double[] E69EF9AC, double 9D896F3D)
		{
			469F9E21.F43F251C.85849ABC 85849ABC = new 469F9E21.F43F251C.85849ABC();
			85849ABC.27ACFCBF = this;
			85849ABC.3109B014 = E69EF9AC;
			List<double[]> list = Enumerable.Range(0, this.AC34C43D).Select(new Func<int, double[]>(85849ABC.23839388)).ToList<double[]>();
			for (int i = this.3C880A28.Count - 1; i >= 0; i--)
			{
				list = this.3C880A28[i].7AA37BAF(list, 9D896F3D);
			}
			List<double[]> list2 = this.B191B336.FE35123B(list);
			List<double[]> list3 = list2;
			bool flag = this.33AB4836 != null;
			if (flag)
			{
				list3 = list2.Select(new Func<double[], double[]>(85849ABC.C3035838)).ToList<double[]>();
			}
			double[] array = new double[this.AC34C43D * this.94994E1D];
			List<double[,]> list4 = new List<double[,]>();
			List<double[]> list5 = new List<double[]>();
			for (int j = 0; j < this.AC34C43D; j++)
			{
				this.E38827AB.7B132382(this.6D939AA6[j], true);
				ValueTuple<double[], double[,], double[]> valueTuple = this.E38827AB.F6B0FEB1(list3[j]);
				double[] item = valueTuple.Item1;
				double[,] item2 = valueTuple.Item2;
				double[] item3 = valueTuple.Item3;
				Array.Copy(item, 0, array, j * this.94994E1D, this.94994E1D);
				list4.Add(item2);
				list5.Add(item3);
			}
			bool flag2;
			if (9D896F3D > 0.0)
			{
				flag2 = list4.Any(new Func<double[,], bool>(469F9E21.F43F251C.E5B28A31.<>9.36BE928C));
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag3)
			{
				double[,] array2 = this.3C8AFF2A(list4.Where(new Func<double[,], bool>(469F9E21.F43F251C.E5B28A31.<>9.BE81CC9F)).ToList<double[,]>());
				double[] array3 = this.DB18399B(list5.Where(new Func<double[], bool>(469F9E21.F43F251C.E5B28A31.<>9.3B1B2415)).ToList<double[]>());
				bool flag4 = array2 != null && array3 != null;
				if (flag4)
				{
					this.E38827AB.208A2188(array2, array3, 9D896F3D);
				}
			}
			return array;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00007DE4 File Offset: 0x000061E4
		private double[,] 3C8AFF2A(List<double[,]> B48D909A)
		{
			bool flag;
			if (B48D909A != null && B48D909A.Count != 0)
			{
				flag = B48D909A.All(new Func<double[,], bool>(469F9E21.F43F251C.E5B28A31.<>9.3F3E3589));
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			double[,] result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				B48D909A = B48D909A.Where(new Func<double[,], bool>(469F9E21.F43F251C.E5B28A31.<>9.55130E9E)).ToList<double[,]>();
				bool flag3 = B48D909A.Count == 0;
				if (flag3)
				{
					result = null;
				}
				else
				{
					int length = B48D909A[0].GetLength(0);
					int length2 = B48D909A[0].GetLength(1);
					double[,] array = new double[length, length2];
					foreach (double[,] array2 in B48D909A)
					{
						for (int i = 0; i < length; i++)
						{
							for (int j = 0; j < length2; j++)
							{
								array[i, j] += array2[i, j];
							}
						}
					}
					for (int k = 0; k < length; k++)
					{
						for (int l = 0; l < length2; l++)
						{
							array[k, l] /= (double)B48D909A.Count;
						}
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00007F64 File Offset: 0x00006364
		private double[] DB18399B(List<double[]> A314E917)
		{
			bool flag;
			if (A314E917 != null && A314E917.Count != 0)
			{
				flag = A314E917.All(new Func<double[], bool>(469F9E21.F43F251C.E5B28A31.<>9.C925FD07));
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			double[] result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				A314E917 = A314E917.Where(new Func<double[], bool>(469F9E21.F43F251C.E5B28A31.<>9.51387003)).ToList<double[]>();
				bool flag3 = A314E917.Count == 0;
				if (flag3)
				{
					result = null;
				}
				else
				{
					int num = A314E917[0].Length;
					double[] array = new double[num];
					foreach (double[] array2 in A314E917)
					{
						for (int i = 0; i < num; i++)
						{
							array[i] += array2[i];
						}
					}
					for (int j = 0; j < num; j++)
					{
						array[j] /= (double)A314E917.Count;
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00008098 File Offset: 0x00006498
		public override object 3A811634()
		{
			return new FF300139<object, List<object>, double>(this.E38827AB.3A811634(), this.3C880A28.Select(new Func<469F9E21.5EA225AE, object>(469F9E21.F43F251C.E5B28A31.<>9.5F0F44BC)).ToList<object>(), this.C031EFA1);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000080EC File Offset: 0x000064EC
		public override void 4486B5B7(JsonElement 5D18C781)
		{
			this.E38827AB.4486B5B7(5D18C781.GetProperty("inputEmbeddingLayer"));
			List<JsonElement> list = 5D18C781.GetProperty("encoderBlocks").EnumerateArray().ToList<JsonElement>();
			for (int i = 0; i < this.3C880A28.Count; i++)
			{
				this.3C880A28[i].CDA2720E(list[i]);
			}
		}

		// Token: 0x04000074 RID: 116
		private readonly int AC34C43D;

		// Token: 0x04000075 RID: 117
		private readonly int 94994E1D;

		// Token: 0x04000076 RID: 118
		private readonly int 15BC9428;

		// Token: 0x04000077 RID: 119
		private readonly 469F9E21.14211E00 E38827AB;

		// Token: 0x04000078 RID: 120
		private readonly 469F9E21.0C9957BA B191B336;

		// Token: 0x04000079 RID: 121
		private readonly List<469F9E21.5EA225AE> 3C880A28;

		// Token: 0x0400007A RID: 122
		private readonly 469F9E21.8482B4BA 33AB4836;

		// Token: 0x0400007B RID: 123
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly double C9BC5F2B;

		// Token: 0x0400007C RID: 124
		private List<double[]> 6D939AA6;

		// Token: 0x0400007D RID: 125
		private List<double[]> B7A9BB3C;

		// Token: 0x02000029 RID: 41
		[CompilerGenerated]
		[Serializable]
		private sealed class E5B28A31
		{
			// Token: 0x0600010E RID: 270 RVA: 0x00008BC5 File Offset: 0x00006FC5
			internal bool 36BE928C(double[,] 210C6F9F)
			{
				return 210C6F9F != null;
			}

			// Token: 0x0600010F RID: 271 RVA: 0x00008BCB File Offset: 0x00006FCB
			internal bool BE81CC9F(double[,] BC9F5A20)
			{
				return BC9F5A20 != null;
			}

			// Token: 0x06000110 RID: 272 RVA: 0x00008BD1 File Offset: 0x00006FD1
			internal bool 3B1B2415(double[] 0BB5872F)
			{
				return 0BB5872F != null;
			}

			// Token: 0x06000111 RID: 273 RVA: 0x00008BD7 File Offset: 0x00006FD7
			internal bool 3F3E3589(double[,] C332AA2F)
			{
				return C332AA2F == null;
			}

			// Token: 0x06000112 RID: 274 RVA: 0x00008BDD File Offset: 0x00006FDD
			internal bool 55130E9E(double[,] F3373100)
			{
				return F3373100 != null;
			}

			// Token: 0x06000113 RID: 275 RVA: 0x00008BE3 File Offset: 0x00006FE3
			internal bool C925FD07(double[] 630EE421)
			{
				return 630EE421 == null;
			}

			// Token: 0x06000114 RID: 276 RVA: 0x00008BE9 File Offset: 0x00006FE9
			internal bool 51387003(double[] 749B0608)
			{
				return 749B0608 != null;
			}

			// Token: 0x06000115 RID: 277 RVA: 0x00008BEF File Offset: 0x00006FEF
			internal object 5F0F44BC(469F9E21.5EA225AE 7187F98D)
			{
				return 7187F98D.889384A3();
			}

			// Token: 0x040000C0 RID: 192
			public static readonly 469F9E21.F43F251C.E5B28A31 <>9 = new 469F9E21.F43F251C.E5B28A31();

			// Token: 0x040000C1 RID: 193
			public static Func<double[,], bool> <>9__14_2;

			// Token: 0x040000C2 RID: 194
			public static Func<double[,], bool> <>9__14_3;

			// Token: 0x040000C3 RID: 195
			public static Func<double[], bool> <>9__14_4;

			// Token: 0x040000C4 RID: 196
			public static Func<double[,], bool> <>9__15_1;

			// Token: 0x040000C5 RID: 197
			public static Func<double[,], bool> <>9__15_0;

			// Token: 0x040000C6 RID: 198
			public static Func<double[], bool> <>9__16_1;

			// Token: 0x040000C7 RID: 199
			public static Func<double[], bool> <>9__16_0;

			// Token: 0x040000C8 RID: 200
			public static Func<469F9E21.5EA225AE, object> <>9__17_0;
		}

		// Token: 0x0200002A RID: 42
		[CompilerGenerated]
		private sealed class 0BAE3EAC
		{
			// Token: 0x06000117 RID: 279 RVA: 0x00008C00 File Offset: 0x00007000
			internal 469F9E21.5EA225AE 989E090A(int E3BE8E1D)
			{
				return new 469F9E21.5EA225AE(this.1B3BD2A1, this.FC1D90BB, this.AEBB2F0C, this.57AF7F13);
			}

			// Token: 0x040000C9 RID: 201
			public int 1B3BD2A1;

			// Token: 0x040000CA RID: 202
			public int FC1D90BB;

			// Token: 0x040000CB RID: 203
			public int AEBB2F0C;

			// Token: 0x040000CC RID: 204
			public double 57AF7F13;
		}

		// Token: 0x0200002B RID: 43
		[CompilerGenerated]
		private sealed class E213D49A
		{
			// Token: 0x06000119 RID: 281 RVA: 0x00008C28 File Offset: 0x00007028
			internal double[] 850AD8A8(double[] 27223A1F)
			{
				return this.06AC4738.33AB4836.7B132382(27223A1F, this.17853590);
			}

			// Token: 0x040000CD RID: 205
			public 469F9E21.F43F251C 06AC4738;

			// Token: 0x040000CE RID: 206
			public bool 17853590;
		}

		// Token: 0x0200002C RID: 44
		[CompilerGenerated]
		private sealed class 85849ABC
		{
			// Token: 0x0600011B RID: 283 RVA: 0x00008C4A File Offset: 0x0000704A
			internal double[] 23839388(int 0A17A719)
			{
				return (0A17A719 == this.27ACFCBF.AC34C43D - 1) ? ((double[])this.3109B014.Clone()) : new double[this.27ACFCBF.15BC9428];
			}

			// Token: 0x0600011C RID: 284 RVA: 0x00008C7E File Offset: 0x0000707E
			internal double[] C3035838(double[] F00A0F15)
			{
				return this.27ACFCBF.33AB4836.721B1D97(F00A0F15, 0.0);
			}

			// Token: 0x040000CF RID: 207
			public 469F9E21.F43F251C 27ACFCBF;

			// Token: 0x040000D0 RID: 208
			public double[] 3109B014;
		}
	}

	// Token: 0x0200001A RID: 26
	public class 1621B7AD
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00008164 File Offset: 0x00006564
		public 1621B7AD()
		{
			this.401C6D85 = new List<469F9E21.FD81572E>
			{
				new 469F9E21.F43F251C(5, 5, 32, 4, 2, 64, 0.1),
				new 469F9E21.8482B4BA(0.1),
				new 469F9E21.14211E00(32, 3, "ActivationHyperbolic")
			};
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000081CC File Offset: 0x000065CC
		public double[] E1B6032E(double[] 500BAA28, bool ABA891AA = false)
		{
			Console.WriteLine("    NN.Forward: Başladı.");
			double[] array = 500BAA28;
			for (int i = 0; i < this.401C6D85.Count; i++)
			{
				469F9E21.FD81572E fd81572E = this.401C6D85[i];
				Console.WriteLine(string.Format("      NN.Forward: Katman {0} ({1}) ileri yayılımı başlıyor...", i, fd81572E.GetType().Name));
				Stopwatch stopwatch = Stopwatch.StartNew();
				array = fd81572E.7B132382(array, ABA891AA);
				stopwatch.Stop();
				Console.WriteLine(string.Format("      NN.Forward: Katman {0} ({1}) ileri yayılımı bitti. Süre: {2}ms", i, fd81572E.GetType().Name, stopwatch.ElapsedMilliseconds));
			}
			Console.WriteLine("    NN.Forward: Bitti.");
			return array;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000828C File Offset: 0x0000668C
		public void 672ADB2D(double[][] 98AB5885, double[][] 852E5B08, int 612BCBBF, double 3CB43FAF, Action<int, double> 5E98133C = null)
		{
			int num = 98AB5885.Length;
			bool flag = num == 0;
			if (flag)
			{
				Console.WriteLine("Eğitim için örnek bulunamadı.");
			}
			else
			{
				Stopwatch stopwatch = new Stopwatch();
				Stopwatch stopwatch2 = new Stopwatch();
				for (int i = 0; i < 612BCBBF; i++)
				{
					Console.WriteLine(string.Format("--- Epoch {0} Başlıyor ---", i));
					stopwatch.Restart();
					double num2 = 0.0;
					for (int j = 0; j < num; j++)
					{
						Console.WriteLine(string.Format("  Epoch {0}, Örnek {1}/{2} işleniyor...", i, j + 1, num));
						stopwatch2.Restart();
						Console.WriteLine(string.Format("    [Örnek {0}] İleri yayılım başlıyor...", j + 1));
						double[] array = this.E1B6032E(98AB5885[j], true);
						stopwatch2.Stop();
						Console.WriteLine(string.Format("    [Örnek {0}] İleri yayılım bitti. Süre: {1}ms", j + 1, stopwatch2.ElapsedMilliseconds));
						stopwatch2.Restart();
						double[] array2 = new double[array.Length];
						double num3 = 0.0;
						for (int k = 0; k < array.Length; k++)
						{
							double num4 = array[k] - 852E5B08[j][k];
							array2[k] = 2.0 * num4;
							num3 += num4 * num4;
						}
						num2 += num3;
						Console.WriteLine(string.Format("    [Örnek {0}] Kayıp hesaplandı. Sample Loss: {1:F7}", j + 1, num3));
						stopwatch2.Stop();
						Console.WriteLine(string.Format("    [Örnek {0}] Kayıp hesaplama süresi: {1}ms", j + 1, stopwatch2.ElapsedMilliseconds));
						stopwatch2.Restart();
						double[] 1B3C1D = array2;
						Console.WriteLine(string.Format("    [Örnek {0}] Geri yayılım başlıyor...", j + 1));
						for (int l = this.401C6D85.Count - 1; l >= 0; l--)
						{
							1B3C1D = this.401C6D85[l].721B1D97(1B3C1D, 3CB43FAF);
						}
						stopwatch2.Stop();
						Console.WriteLine(string.Format("    [Örnek {0}] Geri yayılım bitti. Süre: {1}ms", j + 1, stopwatch2.ElapsedMilliseconds));
						Console.WriteLine(string.Format("  Epoch {0}, Örnek {1}/{2} tamamlandı.", i, j + 1, num));
					}
					stopwatch.Stop();
					double num5 = (num > 0) ? (num2 / (double)num) : 0.0;
					Console.WriteLine(string.Format("--- Epoch {0} Bitti. Ortalama Kayıp: {1:F7}, Toplam Süre: {2:F2} saniye ---", i, num5, stopwatch.Elapsed.TotalSeconds));
					if (5E98133C != null)
					{
						5E98133C(i, num5);
					}
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008570 File Offset: 0x00006970
		public void FFA3B102(string 830E67A9)
		{
			A426B988<object[]> value = new A426B988<object[]>(this.401C6D85.Select(new Func<469F9E21.FD81572E, object>(469F9E21.1621B7AD.FF805E31.<>9.0ABE3F8C)).ToArray<object>());
			File.WriteAllText(830E67A9, JsonSerializer.Serialize<A426B988<object[]>>(value, new JsonSerializerOptions
			{
				WriteIndented = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			}));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000085DC File Offset: 0x000069DC
		public void AC8AA79E(string 9A1AA53D)
		{
			bool flag = !File.Exists(9A1AA53D);
			if (flag)
			{
				Console.WriteLine("Model dosyası bulunamadı: " + 9A1AA53D + ". Varsayılan ağ yapısı kullanılacak.");
			}
			else
			{
				try
				{
					JsonDocument jsonDocument = JsonDocument.Parse(File.ReadAllText(9A1AA53D), default(JsonDocumentOptions));
					List<JsonElement> list = jsonDocument.RootElement.GetProperty("layers").EnumerateArray().ToList<JsonElement>();
					bool flag2 = list.Count == this.401C6D85.Count;
					if (flag2)
					{
						for (int i = 0; i < this.401C6D85.Count; i++)
						{
							this.401C6D85[i].4486B5B7(list[i]);
						}
						Console.WriteLine("Model başarıyla yüklendi: " + 9A1AA53D);
					}
					else
					{
						Console.WriteLine(string.Format("Yüklenen model katman sayısı ({0}) != mevcut yapı ({1}). Varsayılan.", list.Count, this.401C6D85.Count));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(string.Concat(new string[]
					{
						"Model yükleme hatası (",
						9A1AA53D,
						"): ",
						ex.Message,
						". Varsayılan."
					}));
				}
			}
		}

		// Token: 0x0400007E RID: 126
		private readonly List<469F9E21.FD81572E> 401C6D85;

		// Token: 0x0200002D RID: 45
		[CompilerGenerated]
		[Serializable]
		private sealed class FF805E31
		{
			// Token: 0x0600011F RID: 287 RVA: 0x00008CAF File Offset: 0x000070AF
			internal object 0ABE3F8C(469F9E21.FD81572E 8BBA13A8)
			{
				return 8BBA13A8.3A811634();
			}

			// Token: 0x040000D1 RID: 209
			public static readonly 469F9E21.1621B7AD.FF805E31 <>9 = new 469F9E21.1621B7AD.FF805E31();

			// Token: 0x040000D2 RID: 210
			public static Func<469F9E21.FD81572E, object> <>9__4_0;
		}
	}

	// Token: 0x0200001B RID: 27
	[CompilerGenerated]
	[Serializable]
	private sealed class F38157B7
	{
		// Token: 0x060000BE RID: 190 RVA: 0x0000874D File Offset: 0x00006B4D
		internal PointF E4AE7D14([TupleElementNames(new string[]
		{
			"Point",
			"SpeedForNextSegment"
		})] ValueTuple<PointF, double> F9987FA7)
		{
			return F9987FA7.Item1;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00008755 File Offset: 0x00006B55
		internal PointF 3B13BD86([TupleElementNames(new string[]
		{
			"Point",
			"SpeedForNextSegment"
		})] ValueTuple<PointF, double> 92B6A49A)
		{
			return 92B6A49A.Item1;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000875D File Offset: 0x00006B5D
		internal string E9B20113(double 650953AE)
		{
			return 650953AE.ToString("F3");
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000876B File Offset: 0x00006B6B
		internal string 01119A15(double FE884C27)
		{
			return FE884C27.ToString("F3");
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00008779 File Offset: 0x00006B79
		internal double[] C3813935([TupleElementNames(new string[]
		{
			"feats",
			"tgt"
		})] ValueTuple<double[], double[]> 7E068880)
		{
			return 7E068880.Item1;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00008781 File Offset: 0x00006B81
		internal double[] 2FB72616([TupleElementNames(new string[]
		{
			"feats",
			"tgt"
		})] ValueTuple<double[], double[]> 2D06799B)
		{
			return 2D06799B.Item2;
		}

		// Token: 0x0400007F RID: 127
		public static readonly 469F9E21.F38157B7 <>9 = new 469F9E21.F38157B7();

		// Token: 0x04000080 RID: 128
		[TupleElementNames(new string[]
		{
			"Point",
			"SpeedForNextSegment"
		})]
		public static Func<ValueTuple<PointF, double>, PointF> <>9__53_0;

		// Token: 0x04000081 RID: 129
		[TupleElementNames(new string[]
		{
			"Point",
			"SpeedForNextSegment"
		})]
		public static Func<ValueTuple<PointF, double>, PointF> <>9__53_1;

		// Token: 0x04000082 RID: 130
		public static Func<double, string> <>9__55_0;

		// Token: 0x04000083 RID: 131
		public static Func<double, string> <>9__55_1;

		// Token: 0x04000084 RID: 132
		[TupleElementNames(new string[]
		{
			"feats",
			"tgt"
		})]
		public static Func<ValueTuple<double[], double[]>, double[]> <>9__55_2;

		// Token: 0x04000085 RID: 133
		[TupleElementNames(new string[]
		{
			"feats",
			"tgt"
		})]
		public static Func<ValueTuple<double[], double[]>, double[]> <>9__55_3;
	}

	// Token: 0x0200001C RID: 28
	[CompilerGenerated]
	private sealed class 861A9526
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00008792 File Offset: 0x00006B92
		internal void 772EDB05()
		{
			this.B69D0500.A59D2B82("Yetersiz eğitim verisi! En az 50.");
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000087A5 File Offset: 0x00006BA5
		internal void B9B9DC85()
		{
			this.B69D0500.A59D2B82(string.Format("Eğitim başlıyor… {0} örnek. Bu uzun sürebilir.", this.2A8F7825.Length));
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000087CC File Offset: 0x00006BCC
		internal void E891D3BD(int 64027730, double 0A9F561C)
		{
			469F9E21.71318AB7 71318AB = new 469F9E21.71318AB7();
			71318AB.3BAA9A22 = this;
			71318AB.81A501B1 = 64027730;
			71318AB.A5038613 = 0A9F561C;
			this.B69D0500.BeginInvoke(new Action(71318AB.0F0FDD92));
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000880D File Offset: 0x00006C0D
		internal void 798A180F()
		{
			this.B69D0500.Text = "Mouse Predictor (Transformer)";
			this.B69D0500.A59D2B82("Eğitim bitti ve 'mouse_transformer_model_v3.json' kaydedildi!");
			this.B69D0500.BF1C881B = false;
		}

		// Token: 0x04000086 RID: 134
		public 469F9E21 B69D0500;

		// Token: 0x04000087 RID: 135
		public double[][] 2A8F7825;

		// Token: 0x04000088 RID: 136
		public int 1BAFC418;
	}

	// Token: 0x0200001D RID: 29
	[CompilerGenerated]
	private sealed class 71318AB7
	{
		// Token: 0x060000CA RID: 202 RVA: 0x00008848 File Offset: 0x00006C48
		internal void 0F0FDD92()
		{
			this.3BAA9A22.B69D0500.Text = string.Format("Eğitim {0}/{1} – Loss {2:F7}", this.81A501B1 + 1, this.3BAA9A22.1BAFC418, this.A5038613);
		}

		// Token: 0x04000089 RID: 137
		public int 81A501B1;

		// Token: 0x0400008A RID: 138
		public double A5038613;

		// Token: 0x0400008B RID: 139
		public 469F9E21.861A9526 3BAA9A22;
	}
}
