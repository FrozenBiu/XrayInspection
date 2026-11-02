using System;

namespace GUI_XRS
{
	// Token: 0x020000E1 RID: 225
	internal class SimModelForeignBody
	{
		// Token: 0x0600160D RID: 5645 RVA: 0x000864F0 File Offset: 0x000846F0
		private void setThicknessAndBlurArrays(double selecteddiameter, double beltspeed, int voltage, string machinesubtype, string detectorsubtype)
		{
			double num = selecteddiameter / 2.0;
			double num2 = 0.0;
			int num3 = 0;
			this.blurLayerCount = 0;
			bool flag = selecteddiameter > 0.0;
			if (flag)
			{
				bool flag2 = this.SelectedMaterial == 2;
				if (flag2)
				{
					bool flag3 = selecteddiameter == 10.0;
					if (flag3)
					{
						num3 = 21;
						this.blurLayerCount = 2;
					}
					bool flag4 = selecteddiameter == 9.0;
					if (flag4)
					{
						num3 = 19;
						this.blurLayerCount = 2;
					}
					bool flag5 = selecteddiameter == 8.0;
					if (flag5)
					{
						num3 = 17;
						this.blurLayerCount = 2;
					}
					bool flag6 = selecteddiameter == 7.0;
					if (flag6)
					{
						num3 = 15;
						this.blurLayerCount = 2;
					}
					bool flag7 = selecteddiameter == 6.0;
					if (flag7)
					{
						num3 = 13;
						this.blurLayerCount = 2;
					}
					bool flag8 = selecteddiameter == 4.0;
					if (flag8)
					{
						num3 = 9;
						this.blurLayerCount = 2;
					}
					bool flag9 = selecteddiameter == 3.5;
					if (flag9)
					{
						num3 = 8;
						this.blurLayerCount = 2;
					}
					bool flag10 = selecteddiameter == 3.0;
					if (flag10)
					{
						num3 = 8;
						this.blurLayerCount = 2;
					}
					bool flag11 = selecteddiameter == 2.5;
					if (flag11)
					{
						num3 = 7;
						this.blurLayerCount = 2;
					}
					bool flag12 = selecteddiameter == 2.0;
					if (flag12)
					{
						num3 = 6;
						this.blurLayerCount = 2;
					}
					bool flag13 = selecteddiameter == 1.5;
					if (flag13)
					{
						num3 = 5;
						this.blurLayerCount = 99;
					}
					bool flag14 = selecteddiameter < 1.5;
					if (flag14)
					{
						num3 = 4;
						this.blurLayerCount = 99;
					}
				}
				else
				{
					bool flag15 = selecteddiameter == 1.8;
					if (flag15)
					{
						num3 = 5;
						this.blurLayerCount = 2;
					}
					bool flag16 = selecteddiameter == 1.6;
					if (flag16)
					{
						num3 = 5;
						this.blurLayerCount = 2;
					}
					bool flag17 = selecteddiameter == 1.2;
					if (flag17)
					{
						num3 = 4;
						this.blurLayerCount = 2;
					}
					bool flag18 = selecteddiameter == 1.0;
					if (flag18)
					{
						num3 = 4;
						this.blurLayerCount = 2;
					}
					bool flag19 = selecteddiameter == 0.8;
					if (flag19)
					{
						num3 = 4;
						this.blurLayerCount = 2;
					}
					bool flag20 = selecteddiameter < 0.8;
					if (flag20)
					{
						num3 = 3;
						this.blurLayerCount = 0;
					}
				}
				bool flag21 = num3 > 1;
				if (flag21)
				{
					num2 = num / (double)(num3 - 1);
				}
				bool flag22 = machinesubtype.Contains("LW");
				double num4;
				double num5;
				double num6;
				if (flag22)
				{
					num4 = this.calcCompression(beltspeed, true, false);
					num5 = this.calcCompression(beltspeed, true, true);
					num6 = this.calcAdditionalBrightness(beltspeed, true, voltage);
				}
				else
				{
					bool flag23 = this.machineSubType.Contains("TX");
					if (flag23)
					{
						num4 = 1.0;
						num5 = 1.0;
						num6 = this.calcAdditionalBrightness(beltspeed, false, voltage);
					}
					else
					{
						num4 = 1.0;
						bool flag24 = detectorsubtype == "1";
						if (flag24)
						{
							num5 = 1.0;
						}
						else
						{
							num5 = this.calcCompression(beltspeed, false, true);
						}
						num6 = this.calcAdditionalBrightness(beltspeed, false, voltage);
					}
				}
				this.ballThicknessAbsolute = new double[num3, num3];
				for (int i = 0; i < num3; i++)
				{
					for (int j = 0; j < num3; j++)
					{
						this.ballThicknessAbsolute[i, j] = 2.0 * Math.Round(Math.Cos(Math.Asin(Math.Sqrt(Math.Pow(num5 * (double)i * num2, 2.0) + Math.Pow(num4 * (double)j * num2, 2.0)) / num)) * num, 2);
						this.ballThicknessAbsolute[i, j] *= num6;
						bool flag25 = double.IsNaN(this.ballThicknessAbsolute[i, j]);
						if (flag25)
						{
							this.ballThicknessAbsolute[i, j] = 0.0;
						}
					}
				}
				this.totalBallThicknessAverage = new double[2 * (num3 - 2) + 1, 2 * (num3 - 2) + 1];
				int num7 = num3 - 2;
				for (int k = 0; k < num3 - 1; k++)
				{
					for (int l = 0; l < num3 - 1; l++)
					{
						double num8 = Math.Round((this.ballThicknessAbsolute[k, l] + this.ballThicknessAbsolute[k + 1, l] + this.ballThicknessAbsolute[k, l + 1] + this.ballThicknessAbsolute[k + 1, l + 1]) / 4.0, 3);
						bool flag26 = k == 0 && l == 0;
						if (flag26)
						{
							this.totalBallThicknessAverage[num7, num7] = num8;
						}
						else
						{
							this.totalBallThicknessAverage[num7 + k, num7 + l] = num8;
							this.totalBallThicknessAverage[num7 - k, num7 + l] = num8;
							this.totalBallThicknessAverage[num7 + k, num7 - l] = num8;
							this.totalBallThicknessAverage[num7 - k, num7 - l] = num8;
						}
					}
				}
				this.BlurIndicators = new bool[this.totalBallThicknessAverage.GetLength(0) + 2, this.totalBallThicknessAverage.GetLength(0) + 2];
				bool flag27 = this.blurLayerCount == 2;
				if (flag27)
				{
					this.blurOneLayerInBall(this.totalBallThicknessAverage.GetLength(0));
					this.blurOneLayerOutOfBall(this.totalBallThicknessAverage.GetLength(0));
				}
				else
				{
					bool flag28 = this.blurLayerCount == 1;
					if (flag28)
					{
						this.blurOneLayerOutOfBall(this.totalBallThicknessAverage.GetLength(0));
					}
					else
					{
						bool flag29 = this.blurLayerCount == 99;
						if (flag29)
						{
							this.blurTheWholeBall(this.totalBallThicknessAverage.GetLength(0));
						}
					}
				}
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00086B3C File Offset: 0x00084D3C
		private double calcCompression(double beltspeed, bool isLW, bool IsYdirection)
		{
			double num = 1.0;
			if (isLW)
			{
				num = 1.25;
			}
			bool flag = IsYdirection && beltspeed > 0.65;
			if (flag)
			{
				num += 1.4285714285714286 * (beltspeed - 0.65);
			}
			return Math.Round(num, 2);
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00086B9C File Offset: 0x00084D9C
		private double calcAdditionalBrightness(double beltspeed, bool isLW, int voltage)
		{
			double num = 1.0;
			bool flag = this.machineSubType.Contains("TX");
			if (flag)
			{
				bool flag2 = beltspeed > 0.4;
				if (flag2)
				{
					bool flag3 = voltage >= 50 && voltage < 60;
					double num2;
					if (flag3)
					{
						num2 = 0.05;
					}
					else
					{
						num2 = 0.1;
					}
					num -= num2 / 0.6 * (beltspeed - 0.4);
				}
			}
			else if (isLW)
			{
				bool flag4 = this.SelectedMaterial == 1;
				if (flag4)
				{
					num = 0.9;
				}
				else
				{
					bool flag5 = this.SelectedMaterial == 2;
					if (flag5)
					{
						num = 1.1;
					}
				}
			}
			else
			{
				bool flag6 = beltspeed > 0.4;
				if (flag6)
				{
					bool flag7 = voltage == 40;
					double num2;
					if (flag7)
					{
						num2 = 0.2;
					}
					else
					{
						num2 = 0.1;
					}
					num -= num2 / 0.6 * (beltspeed - 0.4);
				}
			}
			return Math.Round(num, 2);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00086CD0 File Offset: 0x00084ED0
		private void convertThicknessToAbsporption(int voltage)
		{
			int length = this.totalBallThicknessAverage.GetLength(0);
			this.AbsorptionFactors = new double[length, length];
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					this.AbsorptionFactors[i, j] = this.calculateAbsorptionFactors(this.SelectedMaterial, this.totalBallThicknessAverage[i, j], voltage);
				}
			}
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00086D48 File Offset: 0x00084F48
		private void blurOneLayerInBall(int length)
		{
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					bool flag = (i == 0 && this.totalBallThicknessAverage[i, j] != 0.0) || (i == length - 1 && this.totalBallThicknessAverage[i, j] != 0.0) || (j == 0 && this.totalBallThicknessAverage[i, j] != 0.0) || (j == length - 1 && this.totalBallThicknessAverage[i, j] != 0.0);
					if (flag)
					{
						this.BlurIndicators[i + 1, j + 1] = true;
					}
					else
					{
						bool flag2 = this.totalBallThicknessAverage[i, j] != 0.0;
						if (flag2)
						{
							this.BlurIndicators[i + 1, j + 1] = this.isZeroInNeighborhood(j, i);
						}
					}
				}
			}
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00086E58 File Offset: 0x00085058
		private void blurOneLayerOutOfBall(int length)
		{
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					bool flag = true;
					bool flag2 = i == 0;
					if (flag2)
					{
						bool flag3 = this.totalBallThicknessAverage[i, j] == 0.0;
						if (flag3)
						{
							this.BlurIndicators[i + 1, j + 1] = this.isEdgePixel(i, j);
						}
						else
						{
							this.BlurIndicators[i, j + 1] = this.isEdgePixel(i, j);
						}
						flag = false;
					}
					bool flag4 = i == length - 1;
					if (flag4)
					{
						bool flag5 = this.totalBallThicknessAverage[i, j] == 0.0;
						if (flag5)
						{
							this.BlurIndicators[i + 1, j + 1] = this.isEdgePixel(i, j);
						}
						else
						{
							this.BlurIndicators[i + 2, j + 1] = this.isEdgePixel(i, j);
						}
						flag = false;
					}
					bool flag6 = j == 0;
					if (flag6)
					{
						bool flag7 = this.totalBallThicknessAverage[i, j] == 0.0;
						if (flag7)
						{
							this.BlurIndicators[i + 1, j + 1] = this.isEdgePixel(i, j);
						}
						else
						{
							this.BlurIndicators[i + 1, j] = this.isEdgePixel(i, j);
						}
						flag = false;
					}
					bool flag8 = j == length - 1;
					if (flag8)
					{
						bool flag9 = this.totalBallThicknessAverage[i, j] == 0.0;
						if (flag9)
						{
							this.BlurIndicators[i + 1, j + 1] = this.isEdgePixel(i, j);
						}
						else
						{
							this.BlurIndicators[i + 1, j + 2] = this.isEdgePixel(i, j);
						}
						flag = false;
					}
					bool flag10 = this.totalBallThicknessAverage[i, j] == 0.0 && flag;
					if (flag10)
					{
						this.BlurIndicators[i + 1, j + 1] = this.isEdgePixel(i, j);
					}
				}
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00087058 File Offset: 0x00085258
		private void blurTheWholeBall(int length)
		{
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					bool flag = this.totalBallThicknessAverage[i, j] != 0.0;
					if (flag)
					{
						this.BlurIndicators[i + 1, j + 1] = this.isEdgePixel(i, j);
					}
				}
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000870C8 File Offset: 0x000852C8
		private bool isEdgePixel(int x, int y)
		{
			int num = this.totalBallThicknessAverage.GetLength(0) - 1;
			bool flag = x > 0 && y > 0 && this.totalBallThicknessAverage[y - 1, x - 1] != 0.0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = x > 0 && y < num && this.totalBallThicknessAverage[y + 1, x - 1] != 0.0;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = x < num && y > 0 && this.totalBallThicknessAverage[y - 1, x + 1] != 0.0;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = x < num && y < num && this.totalBallThicknessAverage[y + 1, x + 1] != 0.0;
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = y > 0 && this.totalBallThicknessAverage[y - 1, x] != 0.0;
							if (flag5)
							{
								result = true;
							}
							else
							{
								bool flag6 = y < num && this.totalBallThicknessAverage[y + 1, x] != 0.0;
								if (flag6)
								{
									result = true;
								}
								else
								{
									bool flag7 = x > 0 && this.totalBallThicknessAverage[y, x - 1] != 0.0;
									if (flag7)
									{
										result = true;
									}
									else
									{
										bool flag8 = x < num && this.totalBallThicknessAverage[y, x + 1] != 0.0;
										result = flag8;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00087280 File Offset: 0x00085480
		private bool isZeroInNeighborhood(int x, int y)
		{
			return this.totalBallThicknessAverage[y, x + 1] == 0.0 || this.totalBallThicknessAverage[y, x - 1] == 0.0 || this.totalBallThicknessAverage[y + 1, x] == 0.0 || this.totalBallThicknessAverage[y - 1, x] == 0.0 || this.totalBallThicknessAverage[y - 1, x - 1] == 0.0 || this.totalBallThicknessAverage[y - 1, x + 1] == 0.0 || this.totalBallThicknessAverage[y + 1, x - 1] == 0.0 || this.totalBallThicknessAverage[y + 1, x + 1] == 0.0;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00087380 File Offset: 0x00085580
		public void LoadParameters(int simmaterial, int simsize, int sourcevolt, double beltspeed, string machinesubtype, string detectorsubtype)
		{
			this.SelectedMaterial = simmaterial;
			this.machineSubType = machinesubtype;
			this.convertDiameterToDouble(simmaterial, "", simsize, true);
			this.setThicknessAndBlurArrays(this.Diameter, beltspeed, sourcevolt, this.machineSubType, detectorsubtype);
			this.convertThicknessToAbsporption(sourcevolt);
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x000873CC File Offset: 0x000855CC
		private void convertDiameterToDouble(int simmaterial, string strsimmaterial, int simsize, bool changeDiameter)
		{
			double diameter = 0.0;
			bool flag = simmaterial == 0 || simmaterial == 1 || strsimmaterial.Trim() == "VA";
			if (flag)
			{
				bool flag2 = simsize == 0;
				if (flag2)
				{
					diameter = 0.3;
				}
				else
				{
					bool flag3 = simsize == 1;
					if (flag3)
					{
						diameter = 0.4;
					}
					else
					{
						bool flag4 = simsize == 2;
						if (flag4)
						{
							diameter = 0.5;
						}
						else
						{
							bool flag5 = simsize == 3;
							if (flag5)
							{
								diameter = 0.6;
							}
							else
							{
								bool flag6 = simsize == 4;
								if (flag6)
								{
									diameter = 0.8;
								}
								else
								{
									bool flag7 = simsize == 5;
									if (flag7)
									{
										diameter = 1.0;
									}
									else
									{
										bool flag8 = simsize == 6;
										if (flag8)
										{
											diameter = 1.2;
										}
										else
										{
											bool flag9 = simsize == 7;
											if (flag9)
											{
												diameter = 1.6;
											}
											else
											{
												bool flag10 = simsize == 8;
												if (flag10)
												{
													diameter = 1.8;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			else
			{
				bool flag11 = simmaterial == 2 || strsimmaterial.Trim() == "Glass";
				if (flag11)
				{
					bool flag12 = simsize == 0;
					if (flag12)
					{
						diameter = 0.6;
					}
					else
					{
						bool flag13 = simsize == 1;
						if (flag13)
						{
							diameter = 0.8;
						}
						else
						{
							bool flag14 = simsize == 2;
							if (flag14)
							{
								diameter = 1.0;
							}
							else
							{
								bool flag15 = simsize == 3;
								if (flag15)
								{
									diameter = 1.2;
								}
								else
								{
									bool flag16 = simsize == 4;
									if (flag16)
									{
										diameter = 1.5;
									}
									else
									{
										bool flag17 = simsize == 5;
										if (flag17)
										{
											diameter = 2.0;
										}
										else
										{
											bool flag18 = simsize == 6;
											if (flag18)
											{
												diameter = 2.5;
											}
											else
											{
												bool flag19 = simsize == 7;
												if (flag19)
												{
													diameter = 3.0;
												}
												else
												{
													bool flag20 = simsize == 8;
													if (flag20)
													{
														diameter = 3.5;
													}
													else
													{
														bool flag21 = simsize == 9;
														if (flag21)
														{
															diameter = 4.0;
														}
														else
														{
															bool flag22 = simsize == 10;
															if (flag22)
															{
																diameter = 6.0;
															}
															else
															{
																bool flag23 = simsize == 11;
																if (flag23)
																{
																	diameter = 7.0;
																}
																else
																{
																	bool flag24 = simsize == 12;
																	if (flag24)
																	{
																		diameter = 8.0;
																	}
																	else
																	{
																		bool flag25 = simsize == 13;
																		if (flag25)
																		{
																			diameter = 9.0;
																		}
																		else
																		{
																			bool flag26 = simsize == 14;
																			if (flag26)
																			{
																				diameter = 10.0;
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				else
				{
					this.Diameter = 0.0;
				}
			}
			if (changeDiameter)
			{
				this.Diameter = diameter;
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000876B4 File Offset: 0x000858B4
		private double calculateAbsorptionFactors(int material, double thickness, int voltage)
		{
			double aval = 1.0;
			double bval = 0.0;
			double cval = 0.0;
			double dval = 0.0;
			bool isexpfunction = false;
			bool flag = this.machineSubType.Contains("TX");
			if (flag)
			{
				bool flag2 = material == 2;
				if (flag2)
				{
					bool flag3 = voltage == 40;
					if (flag3)
					{
						aval = -0.28;
						isexpfunction = true;
					}
					else
					{
						bool flag4 = voltage == 50;
						if (flag4)
						{
							aval = -0.21;
							isexpfunction = true;
						}
						else
						{
							bool flag5 = voltage == 60;
							if (flag5)
							{
								aval = -0.135;
								isexpfunction = true;
							}
						}
					}
				}
				else
				{
					bool flag6 = voltage == 40;
					if (flag6)
					{
						aval = 0.75;
						bval = 3.0;
						dval = 0.2;
					}
					else
					{
						bool flag7 = voltage == 50;
						if (flag7)
						{
							aval = 0.75;
							bval = 1.7;
							dval = 0.3;
						}
						else
						{
							bool flag8 = voltage == 60;
							if (flag8)
							{
								aval = 0.85;
								bval = 1.0;
								dval = 0.35;
							}
						}
					}
				}
			}
			else
			{
				bool flag9 = material == 2;
				if (flag9)
				{
					bool flag10 = voltage == 40;
					if (flag10)
					{
						aval = -0.33;
						isexpfunction = true;
					}
					else
					{
						bool flag11 = voltage == 50;
						if (flag11)
						{
							aval = -0.23;
							isexpfunction = true;
						}
						else
						{
							bool flag12 = voltage == 60;
							if (flag12)
							{
								aval = -0.16;
								isexpfunction = true;
							}
							else
							{
								bool flag13 = voltage == 70;
								if (flag13)
								{
									aval = 72.0;
									bval = 0.09;
									cval = 0.35;
									dval = 11.0;
								}
								else
								{
									bool flag14 = voltage == 80;
									if (flag14)
									{
										aval = 100.0;
										bval = 0.09;
										cval = 0.4;
										dval = 11.0;
									}
									else
									{
										bool flag15 = voltage == 90;
										if (flag15)
										{
											aval = 130.0;
											bval = 0.085;
											dval = 11.0;
										}
										else
										{
											bool flag16 = voltage == 100;
											if (flag16)
											{
												aval = 119.0;
												bval = 0.06;
												dval = 9.0;
											}
										}
									}
								}
							}
						}
					}
				}
				else
				{
					bool flag17 = voltage == 40;
					if (flag17)
					{
						aval = 0.8;
						bval = 3.0;
						dval = 0.2;
					}
					else
					{
						bool flag18 = voltage == 50;
						if (flag18)
						{
							aval = 0.68;
							bval = 1.7;
							dval = 0.3;
						}
						else
						{
							bool flag19 = voltage == 60;
							if (flag19)
							{
								aval = 0.62;
								bval = 1.0;
								dval = 0.35;
							}
							else
							{
								bool flag20 = voltage == 70;
								if (flag20)
								{
									aval = 0.33;
									bval = 0.13;
									dval = 0.19;
								}
								else
								{
									bool flag21 = voltage == 80;
									if (flag21)
									{
										aval = 0.37;
										bval = 0.12;
										dval = 0.19;
									}
									else
									{
										bool flag22 = voltage == 90;
										if (flag22)
										{
											aval = 0.49;
											bval = 0.11;
											dval = 0.19;
										}
										else
										{
											bool flag23 = voltage == 100;
											if (flag23)
											{
												aval = 1.55;
												bval = 0.45;
												dval = 0.4;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return this.getValueFromAbsorptionGraph(thickness, aval, bval, cval, dval, isexpfunction);
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00087A90 File Offset: 0x00085C90
		private double getValueFromAbsorptionGraph(double xval, double aval, double bval, double cval, double dval, bool isexpfunction)
		{
			double result;
			if (isexpfunction)
			{
				result = Math.Exp(aval * xval);
			}
			else
			{
				result = aval / (bval * Math.Pow(xval, 3.0) + cval * Math.Pow(xval, 2.0) + dval * xval + aval);
			}
			return result;
		}

		// Token: 0x04000EA8 RID: 3752
		public int SelectedMaterial;

		// Token: 0x04000EA9 RID: 3753
		private double[,] ballThicknessAbsolute;

		// Token: 0x04000EAA RID: 3754
		private double[,] totalBallThicknessAverage;

		// Token: 0x04000EAB RID: 3755
		public double[,] AbsorptionFactors;

		// Token: 0x04000EAC RID: 3756
		public bool[,] BlurIndicators;

		// Token: 0x04000EAD RID: 3757
		public double Diameter;

		// Token: 0x04000EAE RID: 3758
		private int blurLayerCount;

		// Token: 0x04000EAF RID: 3759
		private string machineSubType;
	}
}
