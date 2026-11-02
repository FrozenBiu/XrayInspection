using System;
using System.Collections.Generic;
using System.Linq;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000E0 RID: 224
	internal class SimModelCreateImage
	{
		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060015FB RID: 5627 RVA: 0x00085573 File Offset: 0x00083773
		// (set) Token: 0x060015FC RID: 5628 RVA: 0x0008557B File Offset: 0x0008377B
		public ushort[] GreyvaluesData { get; set; }

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x00085584 File Offset: 0x00083784
		// (set) Token: 0x060015FE RID: 5630 RVA: 0x0008558C File Offset: 0x0008378C
		public int Height { get; set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x00085595 File Offset: 0x00083795
		// (set) Token: 0x06001600 RID: 5632 RVA: 0x0008559D File Offset: 0x0008379D
		public int Width { get; set; }

		// Token: 0x06001601 RID: 5633 RVA: 0x000855A8 File Offset: 0x000837A8
		public short[] SimulateTestcard(XRSImage inputimage, SimModelForeignBody foreignBody, int imageVoltage, int xpattern, int ypattern, int backgroundnoise, bool simulateAllBalls)
		{
			this.foreignBody = foreignBody;
			short[] array = new short[inputimage.Data.Length];
			short[] array2 = new short[inputimage.Data.Length];
			array2 = inputimage.Data;
			array = inputimage.Data;
			this.BallCounter = 0;
			int num = this.addLayerForEdgeProblem(imageVoltage, (int)inputimage.Width, (int)inputimage.Height);
			int num2 = this.ProductMinX + num;
			int num3 = this.ProductMinY + num;
			double num4 = (double)(this.ProductMaxX - num2 - num);
			double num5 = (double)(this.ProductMaxY - num3 - num);
			int length = foreignBody.AbsorptionFactors.GetLength(0);
			int num6 = (length - 1) / 2;
			xpattern = this.reducePatternIfNecessary(num4, length, xpattern);
			ypattern = this.reducePatternIfNecessary(num5, length, ypattern);
			bool flag = xpattern > 0 && ypattern > 0;
			if (flag)
			{
				this.bodyGapX = Convert.ToInt32(Math.Round(num4 / (double)xpattern));
				this.bodyGapY = Convert.ToInt32(Math.Round(num5 / (double)ypattern));
				num2 += this.bodyGapX / 2 - num6;
				num3 += this.bodyGapY / 2 - num6;
				for (int i = 0; i < ypattern; i++)
				{
					for (int j = 0; j < xpattern; j++)
					{
						int num7 = num2 + j * this.bodyGapX;
						int num8 = num3 + i * this.bodyGapY;
						int num9 = num7 + num6;
						int num10 = num8 + num6;
						bool flag2 = true;
						bool flag3 = num9 + num6 + 7 > this.Height || num10 + num6 + 7 > this.Width || this.checkBackgroundAndEdgeFilter(inputimage, num9, num10, num6, backgroundnoise, simulateAllBalls, num);
						if (flag3)
						{
							flag2 = false;
						}
						bool flag4 = flag2 && inputimage.Type == ImageType.Demo;
						if (flag4)
						{
							flag2 = this.checkMarkedPixel(inputimage, num7, num8, length);
						}
						bool flag5 = flag2;
						if (flag5)
						{
							this.setImageMask(inputimage, num7, num8, length);
							this.BallCounter++;
							for (int k = 0; k < length; k++)
							{
								for (int l = 0; l < length; l++)
								{
									double num11 = foreignBody.AbsorptionFactors[k, l];
									bool flag6 = num11 > 0.0 && num11 < 1.0;
									if (flag6)
									{
										array2[(num8 + k) * this.Height + num7 + l] = Convert.ToInt16(num11 * (double)array2[(num8 + k) * this.Height + num7 + l]);
									}
								}
							}
							int length2 = foreignBody.BlurIndicators.GetLength(0);
							for (int m = 0; m < length2; m++)
							{
								for (int n = 0; n < length2; n++)
								{
									bool flag7 = foreignBody.BlurIndicators[m, n];
									if (flag7)
									{
										int num12 = num7 - 1 + n;
										int num13 = num8 - 1 + m;
										array[num13 * this.Height + num12] = this.blurEdgePixel(array2, num13, num12);
									}
								}
							}
						}
					}
				}
			}
			return array;
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x00085900 File Offset: 0x00083B00
		public short[] SimulateTestcardWithDifferentSizes(XRSImage inputimage, SimModelForeignBody foreignBody, int imageVoltage, int xpattern, int ypattern, int backgroundnoise, bool simulateAllBalls, int simmaterial, List<int> simsize, double beltspeed, string machinesubtype, string detectorsubtype, string machinetype)
		{
			this.foreignBody = foreignBody;
			short[] array = new short[inputimage.Data.Length];
			short[] array2 = new short[inputimage.Data.Length];
			array2 = inputimage.Data;
			array = inputimage.Data;
			this.BallCounter = 0;
			int num = this.addLayerForEdgeProblem(imageVoltage, (int)inputimage.Width, (int)inputimage.Height);
			int num2 = this.ProductMinX + num;
			int num3 = this.ProductMinY + num;
			double num4 = (double)(this.ProductMaxX - num2 - num);
			double num5 = (double)(this.ProductMaxY - num3 - num);
			int index = simsize.Count - 1;
			foreignBody.LoadParameters(simmaterial, simsize[index], imageVoltage, beltspeed, machinesubtype, detectorsubtype);
			int length = foreignBody.AbsorptionFactors.GetLength(0);
			int num6 = (length - 1) / 2;
			xpattern = this.reducePatternIfNecessary(num4, length, xpattern);
			ypattern = this.reducePatternIfNecessary(num5, length, ypattern);
			xpattern = ((xpattern <= 0) ? 1 : xpattern);
			ypattern = ((ypattern <= 0) ? 1 : ypattern);
			this.bodyGapX = Convert.ToInt32(Math.Round(num4 / (double)xpattern));
			this.bodyGapY = Convert.ToInt32(Math.Round(num5 / (double)ypattern));
			num2 += this.bodyGapX / 2 - num6;
			num3 += this.bodyGapY / 2 - num6;
			int num7 = 0;
			for (int i = 0; i < ypattern; i++)
			{
				for (int j = 0; j < xpattern; j++)
				{
					bool flag = num7 >= simsize.Count;
					if (flag)
					{
						num7 = 0;
					}
					foreignBody.LoadParameters(simmaterial, simsize[num7], imageVoltage, beltspeed, machinesubtype, detectorsubtype);
					length = foreignBody.AbsorptionFactors.GetLength(0);
					num6 = (length - 1) / 2;
					num7++;
					int num8 = num2 + j * this.bodyGapX;
					int num9 = num3 + i * this.bodyGapY;
					int num10 = num8 + num6;
					int num11 = num9 + num6;
					bool flag2 = true;
					bool flag3 = num10 + num6 + 7 > this.Height || num11 + num6 + 7 > this.Width || this.checkBackgroundAndEdgeFilter(inputimage, num10, num11, num6, backgroundnoise, simulateAllBalls, num);
					if (flag3)
					{
						flag2 = false;
					}
					bool flag4 = flag2 && inputimage.Type == ImageType.Demo;
					if (flag4)
					{
						flag2 = this.checkMarkedPixel(inputimage, num8, num9, length);
					}
					bool flag5 = flag2;
					if (flag5)
					{
						this.setImageMask(inputimage, num8, num9, length);
						this.BallCounter++;
						for (int k = 0; k < length; k++)
						{
							for (int l = 0; l < length; l++)
							{
								double num12 = foreignBody.AbsorptionFactors[k, l];
								bool flag6 = num12 > 0.0 && num12 < 1.0;
								if (flag6)
								{
									array2[(num9 + k) * this.Height + num8 + l] = Convert.ToInt16(num12 * (double)array2[(num9 + k) * this.Height + num8 + l]);
								}
							}
						}
						int length2 = foreignBody.BlurIndicators.GetLength(0);
						for (int m = 0; m < length2; m++)
						{
							for (int n = 0; n < length2; n++)
							{
								bool flag7 = foreignBody.BlurIndicators[m, n];
								if (flag7)
								{
									int num13 = num8 - 1 + n;
									int num14 = num9 - 1 + m;
									array[num14 * this.Height + num13] = this.blurEdgePixel(array2, num14, num13);
								}
							}
						}
					}
				}
			}
			return array;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00085CCC File Offset: 0x00083ECC
		public short[] SimulateOneDefaultBall(XRSImage inputimage, SimModelForeignBody foreignBody, int imageVoltage, int xpattern, int ypattern, int backgroundnoise, bool simulateAllBalls)
		{
			this.foreignBody = foreignBody;
			short[] array = new short[inputimage.Data.Length];
			short[] array2 = new short[inputimage.Data.Length];
			array2 = inputimage.Data;
			array = inputimage.Data;
			this.BallCounter++;
			int num = (this.ProductMaxX + this.ProductMinX) / 2;
			int num2 = (this.ProductMaxY + this.ProductMinY) / 2;
			int length = foreignBody.AbsorptionFactors.GetLength(0);
			this.setImageMask(inputimage, num, num2, length);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					double num3 = foreignBody.AbsorptionFactors[i, j];
					int num4 = (num2 + i) * this.Height + num + j;
					bool flag = num3 > 0.0 && num3 < 1.0 && num4 > 0 && num4 < array2.Length;
					if (flag)
					{
						array2[num4] = Convert.ToInt16(num3 * (double)array2[num4]);
					}
				}
			}
			int length2 = foreignBody.BlurIndicators.GetLength(0);
			for (int k = 0; k < length2; k++)
			{
				for (int l = 0; l < length2; l++)
				{
					bool flag2 = foreignBody.BlurIndicators[k, l];
					if (flag2)
					{
						int num5 = num - 1 + l;
						int num6 = num2 - 1 + k;
						int num4 = num6 * this.Height + num5;
						bool flag3 = num4 > 0 && num4 < array.Length;
						if (flag3)
						{
							array[num4] = this.blurEdgePixel(array2, num6, num5);
						}
					}
				}
			}
			return array;
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00085EA0 File Offset: 0x000840A0
		private short blurEdgePixel(short[] inputData, int blurpointY, int blurpointX)
		{
			return Convert.ToInt16(new int[]
			{
				(int)inputData[blurpointY * this.Height + blurpointX],
				(int)inputData[(blurpointY - 1) * this.Height + blurpointX - 1],
				(int)inputData[(blurpointY + 1) * this.Height + blurpointX - 1],
				(int)inputData[(blurpointY - 1) * this.Height + blurpointX + 1],
				(int)inputData[(blurpointY + 1) * this.Height + blurpointX + 1],
				(int)inputData[(blurpointY - 1) * this.Height + blurpointX],
				(int)inputData[(blurpointY + 1) * this.Height + blurpointX],
				(int)inputData[blurpointY * this.Height + blurpointX - 1],
				(int)inputData[blurpointY * this.Height + blurpointX + 1]
			}.Sum() / 9);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00085F68 File Offset: 0x00084168
		private bool checkBackgroundAndEdgeFilter(XRSImage inputimage, int centerX, int centerY, int ballradius, int backgroundnoise, bool simallballs, int additionalLayerEdgeProb)
		{
			bool flag = !simallballs;
			if (flag)
			{
				int num = 0;
				bool flag2 = (int)inputimage.Data[centerY * this.Height + centerX] > backgroundnoise;
				if (flag2)
				{
					num++;
				}
				bool flag3 = (int)inputimage.Data[centerY * this.Height + centerX + ballradius] > backgroundnoise;
				if (flag3)
				{
					num++;
				}
				bool flag4 = (int)inputimage.Data[centerY * this.Height + centerX - ballradius] > backgroundnoise;
				if (flag4)
				{
					num++;
				}
				bool flag5 = (int)inputimage.Data[(centerY + ballradius) * this.Height + centerX] > backgroundnoise;
				if (flag5)
				{
					num++;
				}
				bool flag6 = (int)inputimage.Data[(centerY - ballradius) * this.Height + centerX] > backgroundnoise;
				if (flag6)
				{
					num++;
				}
				bool flag7 = num > 1;
				if (flag7)
				{
					return true;
				}
				bool isEdgeFilteredImage = inputimage.IsEdgeFilteredImage;
				if (isEdgeFilteredImage)
				{
					bool flag8 = (inputimage.Mask[centerY * this.Height + centerX] & XRSImageDefs.edgeFilterIndicator) == XRSImageDefs.edgeFilterIndicator || (inputimage.Mask[centerY * this.Height + centerX + ballradius] & XRSImageDefs.edgeFilterIndicator) == XRSImageDefs.edgeFilterIndicator || (inputimage.Mask[centerY * this.Height + centerX - ballradius] & XRSImageDefs.edgeFilterIndicator) == XRSImageDefs.edgeFilterIndicator || (inputimage.Mask[(centerY + ballradius) * this.Height + centerX] & XRSImageDefs.edgeFilterIndicator) == XRSImageDefs.edgeFilterIndicator || (inputimage.Mask[(centerY - ballradius) * this.Height + centerX] & XRSImageDefs.edgeFilterIndicator) == XRSImageDefs.edgeFilterIndicator;
					if (flag8)
					{
						return true;
					}
				}
				additionalLayerEdgeProb += ballradius;
				bool flag9 = centerY * this.Height + centerX + additionalLayerEdgeProb > inputimage.Data.Length || (int)inputimage.Data[centerY * this.Height + centerX + additionalLayerEdgeProb] > backgroundnoise;
				if (flag9)
				{
					return true;
				}
				bool flag10 = centerY * this.Height + centerX - additionalLayerEdgeProb < 0 || (int)inputimage.Data[centerY * this.Height + centerX - additionalLayerEdgeProb] > backgroundnoise;
				if (flag10)
				{
					return true;
				}
				bool flag11 = (centerY + additionalLayerEdgeProb) * this.Height + centerX > inputimage.Data.Length || (int)inputimage.Data[(centerY + additionalLayerEdgeProb) * this.Height + centerX] > backgroundnoise;
				if (flag11)
				{
					return true;
				}
				bool flag12 = (centerY - additionalLayerEdgeProb) * this.Height + centerX < 0 || (int)inputimage.Data[(centerY - additionalLayerEdgeProb) * this.Height + centerX] > backgroundnoise;
				if (flag12)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x000861FC File Offset: 0x000843FC
		private bool checkMarkedPixel(XRSImage inputimage, int actX, int actY, int balldiameter)
		{
			int num = (int)(inputimage.State & 1);
			bool flag = num == 1;
			if (flag)
			{
				balldiameter += 6;
				actX -= 3;
				actY -= 3;
				for (int i = actX; i < actX + balldiameter; i++)
				{
					for (int j = actY; j < actY + balldiameter; j++)
					{
						byte b = inputimage.Mask[j * this.Height + i];
						b &= 7;
						bool flag2 = b > 0;
						if (flag2)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0008628F File Offset: 0x0008448F
		private void TESTFUNCT_CrossInBallPainter(XRSImage inputimage, int edgeX, int edgeY, int radius, int centerX, int centerY)
		{
			radius += 2;
			edgeX -= 2;
			edgeY -= 2;
			inputimage.Data[centerY * this.Height + centerX] = 9999;
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x000862BC File Offset: 0x000844BC
		private void setImageMask(XRSImage inputimage, int actX, int actY, int balldiameter)
		{
			balldiameter += 6;
			actX -= 3;
			actY -= 3;
			bool flag = inputimage.Mask.Length != 0;
			if (flag)
			{
				for (int i = actX; i < actX + balldiameter; i++)
				{
					for (int j = actY; j < actY + balldiameter; j++)
					{
						int num = j * this.Height + i;
						bool flag2 = num > 0 && num < inputimage.Mask.Length;
						if (flag2)
						{
							byte[] mask = inputimage.Mask;
							int num2 = num;
							mask[num2] |= XRSImageDefs.testcardSimulationIndicator;
						}
					}
				}
			}
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00086354 File Offset: 0x00084554
		private int reducePatternIfNecessary(double length, int ballpixeldiameter, int pattern)
		{
			int num = 0;
			bool flag = ballpixeldiameter > 0;
			if (flag)
			{
				bool flag2 = ballpixeldiameter < 23;
				if (flag2)
				{
					num = Convert.ToInt32(Math.Floor(length / (double)(ballpixeldiameter + 6 + 2)));
				}
				else
				{
					num = Convert.ToInt32(Math.Round(length / ((double)ballpixeldiameter * 1.5)));
				}
			}
			bool flag3 = pattern > num;
			if (flag3)
			{
				pattern = num;
			}
			return pattern;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x000863B8 File Offset: 0x000845B8
		private int addLayerForEdgeProblem(int voltage, int imgWidth, int imgHeight)
		{
			int result = 0;
			bool flag = this.ProductMinX + 100 < imgHeight && this.ProductMinY + 100 < imgWidth;
			if (flag)
			{
				bool flag2 = voltage < 60;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					bool flag3 = voltage >= 60 && voltage < 100;
					if (flag3)
					{
						result = 30;
					}
					else
					{
						bool flag4 = voltage == 100;
						if (flag4)
						{
							result = 50;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00086424 File Offset: 0x00084624
		public void SetStartAndEndPointsforPattern(short[] greyValuesData, int backgroundnoise)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			this.ProductMinX = 100000;
			this.ProductMinY = 100000;
			this.ProductMaxX = 0;
			this.ProductMaxY = 0;
			for (int i = 1; i < greyValuesData.Length; i++)
			{
				num++;
				bool flag2 = i % this.Height == 0;
				if (flag2)
				{
					num2++;
					num = 0;
				}
				bool flag3 = (int)greyValuesData[i - 1] < backgroundnoise;
				if (flag3)
				{
					bool flag4 = !flag;
					if (flag4)
					{
						flag = true;
						this.ProductMinY = num2;
					}
					bool flag5 = num < this.ProductMinX;
					if (flag5)
					{
						this.ProductMinX = num;
					}
					bool flag6 = num > this.ProductMaxX;
					if (flag6)
					{
						this.ProductMaxX = num;
					}
					this.ProductMaxY = num2;
				}
			}
		}

		// Token: 0x04000E98 RID: 3736
		private SimModelForeignBody foreignBody;

		// Token: 0x04000E99 RID: 3737
		public const int OffsetImageDataStart = 23;

		// Token: 0x04000E9A RID: 3738
		public const int OffsetWidth = 3;

		// Token: 0x04000E9B RID: 3739
		public const int OffsetHeight = 1;

		// Token: 0x04000E9C RID: 3740
		private int bodyGapX;

		// Token: 0x04000E9D RID: 3741
		private int bodyGapY;

		// Token: 0x04000E9E RID: 3742
		public int ProductMinX;

		// Token: 0x04000E9F RID: 3743
		public int ProductMinY;

		// Token: 0x04000EA0 RID: 3744
		public int ProductMaxX;

		// Token: 0x04000EA1 RID: 3745
		public int ProductMaxY;

		// Token: 0x04000EA2 RID: 3746
		public int BallCounter;

		// Token: 0x04000EA3 RID: 3747
		private const int addLayerForSimCheck = 7;

		// Token: 0x04000EA4 RID: 3748
		private const int addLayerForMask = 6;
	}
}
