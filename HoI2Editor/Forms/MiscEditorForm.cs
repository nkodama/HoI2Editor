using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using HoI2Editor.Models;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     ゲーム設定エディタのフォーム
    /// </summary>
    public partial class MiscEditorForm : Form
    {
        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MiscEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscEditorFormLoad(object sender, EventArgs e)
        {
            // ゲーム設定ファイルを読み込む
            LoadFiles();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            InitEconomy1Items();
            InitEconomy2Items();
            InitEconomy3Items();
            InitIntelligenceItems();
            InitDiplomacyItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            UpdateEconomy1Items();
            UpdateEconomy2Items();
            UpdateEconomy3Items();
            UpdateIntelligenceItems();
            UpdateDiplomacyItems();
        }

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // ゲーム設定ファイルの再読み込みを要求する
            Misc.RequireReload();

            // ゲーム設定ファイルを読み込む
            LoadFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveFiles();
        }

        /// <summary>
        ///     ゲーム設定ファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 編集項目を初期化する
            InitEditableItems();

            // 編集項目を更新する
            UpdateEditableItems();
        }

        /// <summary>
        ///     ゲーム設定ファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 経済1タブ

        /// <summary>
        ///     経済1タブの項目を初期化する
        /// </summary>
        private void InitEconomy1Items()
        {
            // DDA1.3以上固有項目
            if (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130)
            {
                spyMissionDaysLabel.Enabled = true;
                increateIntelligenceLevelDaysLabel.Enabled = true;
                chanceDetectSpyMissionLabel.Enabled = true;
                relationshipsHitDetectedMissionsLabel.Enabled = true;
                showThirdCountrySpyReportsLabel.Enabled = true;
                distanceModifierNeighboursLabel.Enabled = true;
                spyInformationAccuracyModifierLabel.Enabled = true;
                aiPeacetimeSpyMissionsLabel.Enabled = true;
                maxIcCostModifierLabel.Enabled = true;
                aiSpyMissionsCostModifierLabel.Enabled = true;
                aiDiplomacyCostModifierLabel.Enabled = true;
                aiInfluenceModifierLabel.Enabled = true;

                spyMissionDaysTextBox.Enabled = true;
                increateIntelligenceLevelDaysTextBox.Enabled = true;
                chanceDetectSpyMissionTextBox.Enabled = true;
                relationshipsHitDetectedMissionsTextBox.Enabled = true;
                showThirdCountrySpyReportsComboBox.Enabled = true;
                distanceModifierNeighboursTextBox.Enabled = true;
                spyInformationAccuracyModifierTextBox.Enabled = true;
                aiPeacetimeSpyMissionsComboBox.Enabled = true;
                maxIcCostModifierTextBox.Enabled = true;
                aiSpyMissionsCostModifierTextBox.Enabled = true;
                aiDiplomacyCostModifierTextBox.Enabled = true;
                aiInfluenceModifierTextBox.Enabled = true;
            }
            else
            {
                spyMissionDaysLabel.Enabled = false;
                increateIntelligenceLevelDaysLabel.Enabled = false;
                chanceDetectSpyMissionLabel.Enabled = false;
                relationshipsHitDetectedMissionsLabel.Enabled = false;
                showThirdCountrySpyReportsLabel.Enabled = false;
                distanceModifierNeighboursLabel.Enabled = false;
                spyInformationAccuracyModifierLabel.Enabled = false;
                aiPeacetimeSpyMissionsLabel.Enabled = false;
                maxIcCostModifierLabel.Enabled = false;
                aiSpyMissionsCostModifierLabel.Enabled = false;
                aiDiplomacyCostModifierLabel.Enabled = false;
                aiInfluenceModifierLabel.Enabled = false;

                spyMissionDaysTextBox.Enabled = false;
                increateIntelligenceLevelDaysTextBox.Enabled = false;
                chanceDetectSpyMissionTextBox.Enabled = false;
                relationshipsHitDetectedMissionsTextBox.Enabled = false;
                showThirdCountrySpyReportsComboBox.Enabled = false;
                distanceModifierNeighboursTextBox.Enabled = false;
                spyInformationAccuracyModifierTextBox.Enabled = false;
                aiPeacetimeSpyMissionsComboBox.Enabled = false;
                maxIcCostModifierTextBox.Enabled = false;
                aiSpyMissionsCostModifierTextBox.Enabled = false;
                aiDiplomacyCostModifierTextBox.Enabled = false;
                aiInfluenceModifierTextBox.Enabled = false;

                showThirdCountrySpyReportsComboBox.SelectedIndex = -1;
                showThirdCountrySpyReportsComboBox.ResetText();
                aiPeacetimeSpyMissionsComboBox.SelectedIndex = -1;
                aiPeacetimeSpyMissionsComboBox.ResetText();
            }

            // AoD1.07以上固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107)
            {
                nationalismPerManpowerAoDLabel.Enabled = true;
                coreProvinceEfficiencyRiseTimeLabel.Enabled = true;
                restockSpeedLandLabel.Enabled = true;
                restockSpeedAirLabel.Enabled = true;
                restockSpeedNavalLabel.Enabled = true;
                spyCoupDissentModifierLabel.Enabled = true;
                convoyDutyConversionLabel.Enabled = true;
                escortDutyConversionLabel.Enabled = true;
                tpMaxAttachLabel.Enabled = true;
                ssMaxAttachLabel.Enabled = true;
                ssnMaxAttachLabel.Enabled = true;
                ddMaxAttachLabel.Enabled = true;
                clMaxAttachLabel.Enabled = true;
                caMaxAttachLabel.Enabled = true;
                bcMaxAttachLabel.Enabled = true;
                bbMaxAttachLabel.Enabled = true;
                cvlMaxAttachLabel.Enabled = true;
                cvMaxAttachLabel.Enabled = true;
                canChangeIdeasLabel.Enabled = true;

                nationalismPerManpowerAoDTextBox.Enabled = true;
                coreProvinceEfficiencyRiseTimeTextBox.Enabled = true;
                restockSpeedLandTextBox.Enabled = true;
                restockSpeedAirTextBox.Enabled = true;
                restockSpeedNavalTextBox.Enabled = true;
                spyCoupDissentModifierTextBox.Enabled = true;
                convoyDutyConversionTextBox.Enabled = true;
                escortDutyConversionTextBox.Enabled = true;
                tpMaxAttachTextBox.Enabled = true;
                ssMaxAttachTextBox.Enabled = true;
                ssnMaxAttachTextBox.Enabled = true;
                ddMaxAttachTextBox.Enabled = true;
                clMaxAttachTextBox.Enabled = true;
                caMaxAttachTextBox.Enabled = true;
                bcMaxAttachTextBox.Enabled = true;
                bbMaxAttachTextBox.Enabled = true;
                cvlMaxAttachTextBox.Enabled = true;
                cvMaxAttachTextBox.Enabled = true;
                canChangeIdeasComboBox.Enabled = true;
            }
            else
            {
                nationalismPerManpowerAoDLabel.Enabled = false;
                coreProvinceEfficiencyRiseTimeLabel.Enabled = false;
                restockSpeedLandLabel.Enabled = false;
                restockSpeedAirLabel.Enabled = false;
                restockSpeedNavalLabel.Enabled = false;
                spyCoupDissentModifierLabel.Enabled = false;
                convoyDutyConversionLabel.Enabled = false;
                escortDutyConversionLabel.Enabled = false;
                tpMaxAttachLabel.Enabled = false;
                ssMaxAttachLabel.Enabled = false;
                ssnMaxAttachLabel.Enabled = false;
                ddMaxAttachLabel.Enabled = false;
                clMaxAttachLabel.Enabled = false;
                caMaxAttachLabel.Enabled = false;
                bcMaxAttachLabel.Enabled = false;
                bbMaxAttachLabel.Enabled = false;
                cvlMaxAttachLabel.Enabled = false;
                cvMaxAttachLabel.Enabled = false;
                canChangeIdeasLabel.Enabled = false;

                nationalismPerManpowerAoDTextBox.Enabled = false;
                coreProvinceEfficiencyRiseTimeTextBox.Enabled = false;
                restockSpeedLandTextBox.Enabled = false;
                restockSpeedAirTextBox.Enabled = false;
                restockSpeedNavalTextBox.Enabled = false;
                spyCoupDissentModifierTextBox.Enabled = false;
                convoyDutyConversionTextBox.Enabled = false;
                escortDutyConversionTextBox.Enabled = false;
                tpMaxAttachTextBox.Enabled = false;
                ssMaxAttachTextBox.Enabled = false;
                ssnMaxAttachTextBox.Enabled = false;
                ddMaxAttachTextBox.Enabled = false;
                clMaxAttachTextBox.Enabled = false;
                caMaxAttachTextBox.Enabled = false;
                bcMaxAttachTextBox.Enabled = false;
                bbMaxAttachTextBox.Enabled = false;
                cvlMaxAttachTextBox.Enabled = false;
                cvMaxAttachTextBox.Enabled = false;
                canChangeIdeasComboBox.Enabled = false;

                canChangeIdeasComboBox.SelectedIndex = -1;
                canChangeIdeasComboBox.ResetText();
            }
        }

        /// <summary>
        ///     経済1タブの項目を更新する
        /// </summary>
        private void UpdateEconomy1Items()
        {
            // 編集項目の値を更新する
            icToTcRatioTextBox.Text = Misc.Economy.IcToTcRatio.ToString(CultureInfo.InvariantCulture);
            icToTcRatioTextBox.Text = Misc.Economy.IcToTcRatio.ToString(CultureInfo.InvariantCulture);
            icToSuppliesRatioTextBox.Text = Misc.Economy.IcToSuppliesRatio.ToString(CultureInfo.InvariantCulture);
            icToConsumerGoodsRatioTextBox.Text =
                Misc.Economy.IcToConsumerGoodsRatio.ToString(CultureInfo.InvariantCulture);
            icToMoneyRatioTextBox.Text = Misc.Economy.IcToMoneyRatio.ToString(CultureInfo.InvariantCulture);
            maxGearingBonusTextBox.Text = Misc.Economy.MaxGearingBonus.ToString(CultureInfo.InvariantCulture);
            gearingBonusIncrementTextBox.Text = Misc.Economy.GearingBonusIncrement.ToString(CultureInfo.InvariantCulture);
            icMultiplierNonNationalTextBox.Text =
                Misc.Economy.IcMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
            icMultiplierNonOwnedTextBox.Text = Misc.Economy.IcMultiplierNonOwned.ToString(CultureInfo.InvariantCulture);
            tcLoadUndeployedDivisionTextBox.Text =
                Misc.Economy.TcLoadUndeployedDivision.ToString(CultureInfo.InvariantCulture);
            tcLoadOccupiedTextBox.Text = Misc.Economy.TcLoadOccupied.ToString(CultureInfo.InvariantCulture);
            tcLoadMultiplierLandTextBox.Text = Misc.Economy.TcLoadMultiplierLand.ToString(CultureInfo.InvariantCulture);
            tcLoadMultiplierAirTextBox.Text = Misc.Economy.TcLoadMultiplierAir.ToString(CultureInfo.InvariantCulture);
            tcLoadMultiplierNavalTextBox.Text = Misc.Economy.TcLoadMultiplierNaval.ToString(CultureInfo.InvariantCulture);
            tcLoadPartisanTextBox.Text = Misc.Economy.TcLoadPartisan.ToString(CultureInfo.InvariantCulture);
            tcLoadFactorOffensiveTextBox.Text = Misc.Economy.TcLoadFactorOffensive.ToString(CultureInfo.InvariantCulture);
            tcLoadProvinceDevelopmentTextBox.Text =
                Misc.Economy.TcLoadProvinceDevelopment.ToString(CultureInfo.InvariantCulture);
            tcLoadBaseTextBox.Text = Misc.Economy.TcLoadBase.ToString(CultureInfo.InvariantCulture);
            manpowerMultiplierNationalTextBox.Text =
                Misc.Economy.ManpowerMultiplierNational.ToString(CultureInfo.InvariantCulture);
            manpowerMultiplierNonNationalTextBox.Text =
                Misc.Economy.ManpowerMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
            manpowerMultiplierColonyTextBox.Text =
                Misc.Economy.ManpowerMultiplierColony.ToString(CultureInfo.InvariantCulture);
            requirementAffectSliderTextBox.Text =
                Misc.Economy.RequirementAffectSlider.ToString(CultureInfo.InvariantCulture);
            trickleBackFactorManpowerTextBox.Text =
                Misc.Economy.TrickleBackFactorManpower.ToString(CultureInfo.InvariantCulture);
            reinforceManpowerTextBox.Text = Misc.Economy.ReinforceManpower.ToString(CultureInfo.InvariantCulture);
            reinforceCostTextBox.Text = Misc.Economy.ReinforceCost.ToString(CultureInfo.InvariantCulture);
            reinforceTimeTextBox.Text = Misc.Economy.ReinforceTime.ToString(CultureInfo.InvariantCulture);
            upgradeCostTextBox.Text = Misc.Economy.UpgradeCost.ToString(CultureInfo.InvariantCulture);
            upgradeTimeTextBox.Text = Misc.Economy.UpgradeTime.ToString(CultureInfo.InvariantCulture);
            nationalismStartingValueTextBox.Text =
                Misc.Economy.NationalismStartingValue.ToString(CultureInfo.InvariantCulture);
            monthlyNationalismReductionTextBox.Text =
                Misc.Economy.MonthlyNationalismReduction.ToString(CultureInfo.InvariantCulture);
            sendDivisionDaysTextBox.Text = Misc.Economy.SendDivisionDays.ToString(CultureInfo.InvariantCulture);
            tcLoadUndeployedBrigadeTextBox.Text =
                Misc.Economy.TcLoadUndeployedBrigade.ToString(CultureInfo.InvariantCulture);
            canUnitSendNonAlliedComboBox.SelectedIndex = Misc.Economy.CanUnitSendNonAllied ? 1 : 0;

            if (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130)
            {
                spyMissionDaysTextBox.Text = Misc.Economy.SpyMissionDays.ToString(CultureInfo.InvariantCulture);
                increateIntelligenceLevelDaysTextBox.Text =
                    Misc.Economy.IncreateIntelligenceLevelDays.ToString(CultureInfo.InvariantCulture);
                chanceDetectSpyMissionTextBox.Text =
                    Misc.Economy.ChanceDetectSpyMission.ToString(CultureInfo.InvariantCulture);
                relationshipsHitDetectedMissionsTextBox.Text =
                    Misc.Economy.RelationshipsHitDetectedMissions.ToString(CultureInfo.InvariantCulture);
                showThirdCountrySpyReportsComboBox.SelectedIndex = Misc.Economy.ShowThirdCountrySpyReports;
                distanceModifierNeighboursTextBox.Text =
                    Misc.Economy.DistanceModifierNeighbours.ToString(CultureInfo.InvariantCulture);
                spyInformationAccuracyModifierTextBox.Text =
                    Misc.Economy.SpyInformationAccuracyModifier.ToString(CultureInfo.InvariantCulture);
                aiPeacetimeSpyMissionsComboBox.SelectedIndex = Misc.Economy.AiPeacetimeSpyMissions;
                maxIcCostModifierTextBox.Text = Misc.Economy.MaxIcCostModifier.ToString(CultureInfo.InvariantCulture);
                aiSpyMissionsCostModifierTextBox.Text =
                    Misc.Economy.AiSpyMissionsCostModifier.ToString(CultureInfo.InvariantCulture);
                aiDiplomacyCostModifierTextBox.Text =
                    Misc.Economy.AiDiplomacyCostModifier.ToString(CultureInfo.InvariantCulture);
                aiInfluenceModifierTextBox.Text = Misc.Economy.AiInfluenceModifier.ToString(CultureInfo.InvariantCulture);
            }

            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107)
            {
                nationalismPerManpowerAoDTextBox.Text =
                    Misc.Economy.NationalismPerManpowerAoD.ToString(CultureInfo.InvariantCulture);
                coreProvinceEfficiencyRiseTimeTextBox.Text =
                    Misc.Economy.CoreProvinceEfficiencyRiseTime.ToString(CultureInfo.InvariantCulture);
                restockSpeedLandTextBox.Text = Misc.Economy.RestockSpeedLand.ToString(CultureInfo.InvariantCulture);
                restockSpeedAirTextBox.Text = Misc.Economy.RestockSpeedAir.ToString(CultureInfo.InvariantCulture);
                restockSpeedNavalTextBox.Text = Misc.Economy.RestockSpeedNaval.ToString(CultureInfo.InvariantCulture);
                spyCoupDissentModifierTextBox.Text =
                    Misc.Economy.SpyCoupDissentModifier.ToString(CultureInfo.InvariantCulture);
                convoyDutyConversionTextBox.Text =
                    Misc.Economy.ConvoyDutyConversion.ToString(CultureInfo.InvariantCulture);
                escortDutyConversionTextBox.Text =
                    Misc.Economy.EscortDutyConversion.ToString(CultureInfo.InvariantCulture);
                tpMaxAttachTextBox.Text = Misc.Economy.TpMaxAttach.ToString(CultureInfo.InvariantCulture);
                ssMaxAttachTextBox.Text = Misc.Economy.SsMaxAttach.ToString(CultureInfo.InvariantCulture);
                ssnMaxAttachTextBox.Text = Misc.Economy.SsnMaxAttach.ToString(CultureInfo.InvariantCulture);
                ddMaxAttachTextBox.Text = Misc.Economy.DdMaxAttach.ToString(CultureInfo.InvariantCulture);
                clMaxAttachTextBox.Text = Misc.Economy.ClMaxAttach.ToString(CultureInfo.InvariantCulture);
                caMaxAttachTextBox.Text = Misc.Economy.CaMaxAttach.ToString(CultureInfo.InvariantCulture);
                bcMaxAttachTextBox.Text = Misc.Economy.BcMaxAttach.ToString(CultureInfo.InvariantCulture);
                bbMaxAttachTextBox.Text = Misc.Economy.BbMaxAttach.ToString(CultureInfo.InvariantCulture);
                cvlMaxAttachTextBox.Text = Misc.Economy.CvlMaxAttach.ToString(CultureInfo.InvariantCulture);
                cvMaxAttachTextBox.Text = Misc.Economy.CvMaxAttach.ToString(CultureInfo.InvariantCulture);
                canChangeIdeasComboBox.SelectedIndex = Misc.Economy.CanChangeIdeas ? 1 : 0;
            }

            // 編集項目の色を更新する
            icToTcRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToTcRatio) ? Color.Red : SystemColors.WindowText;
            icToSuppliesRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToSuppliesRatio)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            icToConsumerGoodsRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToConsumerGoodsRatio)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            icToMoneyRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToMoneyRatio)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            maxGearingBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxGearingBonus)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            gearingBonusIncrementTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingBonusIncrement)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            icMultiplierNonNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcMultiplierNonNational)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            icMultiplierNonOwnedTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcMultiplierNonOwned)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            tcLoadUndeployedDivisionTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadUndeployedDivision)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
            tcLoadOccupiedTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadOccupied)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            tcLoadMultiplierLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadMultiplierLand)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            tcLoadMultiplierAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadMultiplierAir)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
            tcLoadMultiplierNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadMultiplierNaval)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            tcLoadPartisanTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadPartisan)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            tcLoadFactorOffensiveTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadFactorOffensive)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            tcLoadProvinceDevelopmentTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadProvinceDevelopment)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            tcLoadBaseTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadBase) ? Color.Red : SystemColors.WindowText;
            manpowerMultiplierNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierNational)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            manpowerMultiplierNonNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierNonNational)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
            manpowerMultiplierColonyTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierColony)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
            requirementAffectSliderTextBox.ForeColor = Misc.IsDirty(MiscItemId.RequirementAffectSlider)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            trickleBackFactorManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.TrickleBackFactorManpower)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            reinforceManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceManpower)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            reinforceCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceCost)
                                                 ? Color.Red
                                                 : SystemColors.WindowText;
            reinforceTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceTime)
                                                 ? Color.Red
                                                 : SystemColors.WindowText;
            upgradeCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.UpgradeCost) ? Color.Red : SystemColors.WindowText;
            upgradeTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.UpgradeTime) ? Color.Red : SystemColors.WindowText;
            nationalismStartingValueTextBox.ForeColor = Misc.IsDirty(MiscItemId.NationalismStartingValue)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
            monthlyNationalismReductionTextBox.ForeColor = Misc.IsDirty(MiscItemId.MonthlyNationalismReduction)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            sendDivisionDaysTextBox.ForeColor = Misc.IsDirty(MiscItemId.SendDivisionDays)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
            tcLoadUndeployedBrigadeTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadUndeployedBrigade)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;

            if (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130)
            {
                spyMissionDaysTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyMissionDays)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                increateIntelligenceLevelDaysTextBox.ForeColor = Misc.IsDirty(MiscItemId.IncreateIntelligenceLevelDays)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                chanceDetectSpyMissionTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceDetectSpyMission)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                relationshipsHitDetectedMissionsTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.RelationshipsHitDetectedMissions) ? Color.Red : SystemColors.WindowText;
                distanceModifierNeighboursTextBox.ForeColor = Misc.IsDirty(MiscItemId.DistanceModifierNeighbours)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                spyInformationAccuracyModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyInformationAccuracyModifier)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                maxIcCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxIcCostModifier)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                aiSpyMissionsCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiSpyMissionsCostModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                aiDiplomacyCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiDiplomacyCostModifier)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                aiInfluenceModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiInfluenceModifier)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            }

            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107)
            {
                nationalismPerManpowerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.NationalismPerManpowerAoD)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                coreProvinceEfficiencyRiseTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.CoreProvinceEfficiencyRiseTime)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                restockSpeedLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.RestockSpeedLand)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                restockSpeedAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.RestockSpeedAir)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                restockSpeedNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.RestockSpeedNaval)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                spyCoupDissentModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyCoupDissentModifier)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                convoyDutyConversionTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyDutyConversion)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                escortDutyConversionTextBox.ForeColor = Misc.IsDirty(MiscItemId.EscortDutyConversion)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                tpMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.TpMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                ssMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.SsMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                ssnMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.SsnMaxAttach)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                ddMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.DdMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                clMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.ClMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                caMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.CaMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                bcMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.BcMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                bbMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.BbMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                cvlMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.CvlMaxAttach)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                cvMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.CvMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     [ICからTCへの変換効率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcToTcRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icToTcRatioTextBox.Text, out val))
            {
                icToTcRatioTextBox.Text = Misc.Economy.IcToTcRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icToTcRatioTextBox.Text = Misc.Economy.IcToTcRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcToTcRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcToTcRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcToTcRatio);
            Misc.SetDirty();

            // 文字色を変更する
            icToTcRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [ICから物資への変換効率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcToSuppliesRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icToSuppliesRatioTextBox.Text, out val))
            {
                icToSuppliesRatioTextBox.Text = Misc.Economy.IcToSuppliesRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icToSuppliesRatioTextBox.Text = Misc.Economy.IcToSuppliesRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcToSuppliesRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcToSuppliesRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcToSuppliesRatio);
            Misc.SetDirty();

            // 文字色を変更する
            icToSuppliesRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [ICから消費財への変換効率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcToConsumerGoodsRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icToConsumerGoodsRatioTextBox.Text, out val))
            {
                icToConsumerGoodsRatioTextBox.Text =
                    Misc.Economy.IcToConsumerGoodsRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icToConsumerGoodsRatioTextBox.Text = Misc.Economy.IcToConsumerGoodsRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcToConsumerGoodsRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcToConsumerGoodsRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcToConsumerGoodsRatio);
            Misc.SetDirty();

            // 文字色を変更する
            icToConsumerGoodsRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [ICから資金への変換効率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcToMoneyRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icToMoneyRatioTextBox.Text, out val))
            {
                icToMoneyRatioTextBox.Text = Misc.Economy.IcToMoneyRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icToMoneyRatioTextBox.Text = Misc.Economy.IcToMoneyRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcToMoneyRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcToMoneyRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcToMoneyRatio);
            Misc.SetDirty();

            // 文字色を変更する
            icToMoneyRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [最大ギアリングボーナス]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxGearingBonusTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxGearingBonusTextBox.Text, out val))
            {
                maxGearingBonusTextBox.Text = Misc.Economy.MaxGearingBonus.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxGearingBonusTextBox.Text = Misc.Economy.MaxGearingBonus.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxGearingBonus) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxGearingBonus = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxGearingBonus);
            Misc.SetDirty();

            // 文字色を変更する
            maxGearingBonusTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [ギアリングボーナスの増加値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGearingBonusIncrementTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(gearingBonusIncrementTextBox.Text, out val))
            {
                gearingBonusIncrementTextBox.Text =
                    Misc.Economy.GearingBonusIncrement.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                gearingBonusIncrementTextBox.Text = Misc.Economy.GearingBonusIncrement.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.GearingBonusIncrement) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.GearingBonusIncrement = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.GearingBonusIncrement);
            Misc.SetDirty();

            // 文字色を変更する
            gearingBonusIncrementTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [非中核州のIC補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcMultiplierNonNationalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icMultiplierNonNationalTextBox.Text, out val))
            {
                icMultiplierNonNationalTextBox.Text =
                    Misc.Economy.IcMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icMultiplierNonNationalTextBox.Text = Misc.Economy.IcMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcMultiplierNonNational) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcMultiplierNonNational = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcMultiplierNonNational);
            Misc.SetDirty();

            // 文字色を変更する
            icMultiplierNonNationalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [占領地のIC補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcMultiplierNonOwnedTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icMultiplierNonOwnedTextBox.Text, out val))
            {
                icMultiplierNonOwnedTextBox.Text =
                    Misc.Economy.IcMultiplierNonOwned.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icMultiplierNonOwnedTextBox.Text = Misc.Economy.IcMultiplierNonOwned.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcMultiplierNonOwned) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcMultiplierNonOwned = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcMultiplierNonOwned);
            Misc.SetDirty();

            // 文字色を変更する
            icMultiplierNonOwnedTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [未配備師団のTC負荷]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadUndeployedDivisionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadUndeployedDivisionTextBox.Text, out val))
            {
                tcLoadUndeployedDivisionTextBox.Text =
                    Misc.Economy.TcLoadUndeployedDivision.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadUndeployedDivisionTextBox.Text = Misc.Economy.TcLoadUndeployedDivision.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadUndeployedDivision) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadUndeployedDivision = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadUndeployedDivision);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadUndeployedDivisionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [占領地のTC負荷]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadOccupiedTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadOccupiedTextBox.Text, out val))
            {
                tcLoadOccupiedTextBox.Text = Misc.Economy.TcLoadOccupied.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadOccupiedTextBox.Text = Misc.Economy.TcLoadOccupied.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadOccupied) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadOccupied = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadOccupied);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadOccupiedTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [陸軍師団のTC負荷補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadMultiplierLandTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadMultiplierLandTextBox.Text, out val))
            {
                tcLoadMultiplierLandTextBox.Text =
                    Misc.Economy.TcLoadMultiplierLand.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadMultiplierLandTextBox.Text = Misc.Economy.TcLoadMultiplierLand.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadMultiplierLand) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadMultiplierLand = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadMultiplierLand);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadMultiplierLandTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [空軍師団のTC負荷補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadMultiplierAirTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadMultiplierAirTextBox.Text, out val))
            {
                tcLoadMultiplierAirTextBox.Text = Misc.Economy.TcLoadMultiplierAir.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadMultiplierAirTextBox.Text = Misc.Economy.TcLoadMultiplierAir.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadMultiplierAir) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadMultiplierAir = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadMultiplierAir);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadMultiplierAirTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [海軍師団のTC負荷補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadMultiplierNavalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadMultiplierNavalTextBox.Text, out val))
            {
                tcLoadMultiplierNavalTextBox.Text =
                    Misc.Economy.TcLoadMultiplierNaval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadMultiplierNavalTextBox.Text = Misc.Economy.TcLoadMultiplierNaval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadMultiplierNaval) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadMultiplierNaval = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadMultiplierNaval);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadMultiplierNavalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [パルチザンのTC負荷]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadPartisanTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadPartisanTextBox.Text, out val))
            {
                tcLoadPartisanTextBox.Text = Misc.Economy.TcLoadPartisan.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadPartisanTextBox.Text = Misc.Economy.TcLoadPartisan.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadPartisan) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadPartisan = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadPartisan);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadPartisanTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [攻勢時のTC負荷係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadFactorOffensiveTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadFactorOffensiveTextBox.Text, out val))
            {
                tcLoadFactorOffensiveTextBox.Text =
                    Misc.Economy.TcLoadFactorOffensive.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadFactorOffensiveTextBox.Text = Misc.Economy.TcLoadFactorOffensive.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadFactorOffensive) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadFactorOffensive = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadFactorOffensive);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadFactorOffensiveTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [プロヴィンス開発のTC負荷]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadProvinceDevelopmentTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadProvinceDevelopmentTextBox.Text, out val))
            {
                tcLoadProvinceDevelopmentTextBox.Text =
                    Misc.Economy.TcLoadProvinceDevelopment.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadProvinceDevelopmentTextBox.Text = Misc.Economy.TcLoadProvinceDevelopment.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadProvinceDevelopment) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadProvinceDevelopment = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadProvinceDevelopment);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadProvinceDevelopmentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [未配備の基地のTC負荷]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadBaseTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadBaseTextBox.Text, out val))
            {
                tcLoadBaseTextBox.Text = Misc.Economy.TcLoadBase.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadBaseTextBox.Text = Misc.Economy.TcLoadBase.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadBase) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadBase = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadBase);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadBaseTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [中核州の人的資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerMultiplierNationalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerMultiplierNationalTextBox.Text, out val))
            {
                manpowerMultiplierNationalTextBox.Text =
                    Misc.Economy.ManpowerMultiplierNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerMultiplierNationalTextBox.Text = Misc.Economy.ManpowerMultiplierNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerMultiplierNational) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerMultiplierNational = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerMultiplierNational);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerMultiplierNationalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [非中核州の人的資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerMultiplierNonNationalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerMultiplierNonNationalTextBox.Text, out val))
            {
                manpowerMultiplierNonNationalTextBox.Text =
                    Misc.Economy.ManpowerMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerMultiplierNonNationalTextBox.Text = Misc.Economy.ManpowerMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerMultiplierNonNational) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerMultiplierNonNational = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerMultiplierNonNational);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerMultiplierNonNationalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [海外州の人的資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerMultiplierColonyTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerMultiplierColonyTextBox.Text, out val))
            {
                manpowerMultiplierColonyTextBox.Text =
                    Misc.Economy.ManpowerMultiplierColony.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerMultiplierColonyTextBox.Text = Misc.Economy.ManpowerMultiplierColony.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerMultiplierColony) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerMultiplierColony = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerMultiplierColony);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerMultiplierColonyTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [政策スライダーに影響を与えるためのIC比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRequirementAffectSliderTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(requirementAffectSliderTextBox.Text, out val))
            {
                requirementAffectSliderTextBox.Text =
                    Misc.Economy.RequirementAffectSlider.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                requirementAffectSliderTextBox.Text = Misc.Economy.RequirementAffectSlider.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.RequirementAffectSlider) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.RequirementAffectSlider = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RequirementAffectSlider);
            Misc.SetDirty();

            // 文字色を変更する
            requirementAffectSliderTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [戦闘による損失からの復帰係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTrickleBackFactorManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(trickleBackFactorManpowerTextBox.Text, out val))
            {
                trickleBackFactorManpowerTextBox.Text =
                    Misc.Economy.TrickleBackFactorManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                trickleBackFactorManpowerTextBox.Text = Misc.Economy.TrickleBackFactorManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TrickleBackFactorManpower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TrickleBackFactorManpower = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TrickleBackFactorManpower);
            Misc.SetDirty();

            // 文字色を変更する
            trickleBackFactorManpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [補充に必要な人的資源の比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(reinforceManpowerTextBox.Text, out val))
            {
                reinforceManpowerTextBox.Text = Misc.Economy.ReinforceManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                reinforceManpowerTextBox.Text = Misc.Economy.ReinforceManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ReinforceManpower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ReinforceManpower = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ReinforceManpower);
            Misc.SetDirty();

            // 文字色を変更する
            reinforceManpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [補充に必要なICの比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(reinforceCostTextBox.Text, out val))
            {
                reinforceCostTextBox.Text = Misc.Economy.ReinforceCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                reinforceCostTextBox.Text = Misc.Economy.ReinforceCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ReinforceCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ReinforceCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ReinforceCost);
            Misc.SetDirty();

            // 文字色を変更する
            reinforceCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [補充に必要な時間の比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(reinforceTimeTextBox.Text, out val))
            {
                reinforceTimeTextBox.Text = Misc.Economy.ReinforceTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                reinforceTimeTextBox.Text = Misc.Economy.ReinforceTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ReinforceTime) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ReinforceTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ReinforceTime);
            Misc.SetDirty();

            // 文字色を変更する
            reinforceTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [改良に必要なICの比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(upgradeCostTextBox.Text, out val))
            {
                upgradeCostTextBox.Text = Misc.Economy.UpgradeCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                upgradeCostTextBox.Text = Misc.Economy.UpgradeCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.UpgradeCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.UpgradeCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.UpgradeCost);
            Misc.SetDirty();

            // 文字色を変更する
            upgradeCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [改良に必要な時間の比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(upgradeTimeTextBox.Text, out val))
            {
                upgradeTimeTextBox.Text = Misc.Economy.UpgradeTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                upgradeTimeTextBox.Text = Misc.Economy.UpgradeTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.UpgradeTime) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.UpgradeTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.UpgradeTime);
            Misc.SetDirty();

            // 文字色を変更する
            upgradeTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [ナショナリズムの初期値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNationalismStartingValueTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(nationalismStartingValueTextBox.Text, out val))
            {
                nationalismStartingValueTextBox.Text =
                    Misc.Economy.NationalismStartingValue.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                nationalismStartingValueTextBox.Text = Misc.Economy.NationalismStartingValue.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.NationalismStartingValue) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.NationalismStartingValue = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.NationalismStartingValue);
            Misc.SetDirty();

            // 文字色を変更する
            nationalismStartingValueTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [月ごとのナショナリズムの減少値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMonthlyNationalismReductionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(monthlyNationalismReductionTextBox.Text, out val))
            {
                monthlyNationalismReductionTextBox.Text =
                    Misc.Economy.MonthlyNationalismReduction.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                monthlyNationalismReductionTextBox.Text = Misc.Economy.MonthlyNationalismReduction.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MonthlyNationalismReduction) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MonthlyNationalismReduction = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MonthlyNationalismReduction);
            Misc.SetDirty();

            // 文字色を変更する
            monthlyNationalismReductionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [師団譲渡後配備可能になるまでの時間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendDivisionDaysTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(sendDivisionDaysTextBox.Text, out val))
            {
                sendDivisionDaysTextBox.Text = Misc.Economy.SendDivisionDays.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                sendDivisionDaysTextBox.Text = Misc.Economy.SendDivisionDays.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.SendDivisionDays)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SendDivisionDays = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SendDivisionDays);
            Misc.SetDirty();

            // 文字色を変更する
            sendDivisionDaysTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [未配備旅団のTC負荷]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcLoadUndeployedBrigadeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tcLoadUndeployedBrigadeTextBox.Text, out val))
            {
                tcLoadUndeployedBrigadeTextBox.Text =
                    Misc.Economy.TcLoadUndeployedBrigade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tcLoadUndeployedBrigadeTextBox.Text = Misc.Economy.TcLoadUndeployedBrigade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TcLoadUndeployedBrigade) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TcLoadUndeployedBrigade = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TcLoadUndeployedBrigade);
            Misc.SetDirty();

            // 文字色を変更する
            tcLoadUndeployedBrigadeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [非同盟国に師団を譲渡できるかどうか]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanUnitSendNonAlliedComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.CanUnitSendNonAllied ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.CanUnitSendNonAllied))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = canUnitSendNonAlliedComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [非同盟国に師団を譲渡できるかどうか]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanUnitSendNonAlliedComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (canUnitSendNonAlliedComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (canUnitSendNonAlliedComboBox.SelectedIndex == 1);
            if (val == Misc.Economy.CanUnitSendNonAllied)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CanUnitSendNonAllied = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CanUnitSendNonAllied);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            canUnitSendNonAlliedComboBox.Refresh();
        }

        /// <summary>
        ///     [諜報任務の間隔]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyMissionDaysTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(spyMissionDaysTextBox.Text, out val))
            {
                spyMissionDaysTextBox.Text = Misc.Economy.SpyMissionDays.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyMissionDaysTextBox.Text = Misc.Economy.SpyMissionDays.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.SpyMissionDays)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SpyMissionDays = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyMissionDays);
            Misc.SetDirty();

            // 文字色を変更する
            spyMissionDaysTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [諜報レベルの増加間隔]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIncreateIntelligenceLevelDaysTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(increateIntelligenceLevelDaysTextBox.Text, out val))
            {
                increateIntelligenceLevelDaysTextBox.Text =
                    Misc.Economy.IncreateIntelligenceLevelDays.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                increateIntelligenceLevelDaysTextBox.Text = Misc.Economy.IncreateIntelligenceLevelDays.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.IncreateIntelligenceLevelDays)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IncreateIntelligenceLevelDays = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IncreateIntelligenceLevelDays);
            Misc.SetDirty();

            // 文字色を変更する
            increateIntelligenceLevelDaysTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [国内の諜報活動を発見する確率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChanceDetectSpyMissionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(chanceDetectSpyMissionTextBox.Text, out val))
            {
                chanceDetectSpyMissionTextBox.Text =
                    Misc.Economy.ChanceDetectSpyMission.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0 || val > 100)
            {
                chanceDetectSpyMissionTextBox.Text = Misc.Economy.ChanceDetectSpyMission.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.ChanceDetectSpyMission)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ChanceDetectSpyMission = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ChanceDetectSpyMission);
            Misc.SetDirty();

            // 文字色を変更する
            chanceDetectSpyMissionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [諜報任務発覚時の友好度低下量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationshipsHitDetectedMissionsTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(relationshipsHitDetectedMissionsTextBox.Text, out val))
            {
                relationshipsHitDetectedMissionsTextBox.Text =
                    Misc.Economy.RelationshipsHitDetectedMissions.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0 || val > 400)
            {
                relationshipsHitDetectedMissionsTextBox.Text = Misc.Economy.RelationshipsHitDetectedMissions.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.RelationshipsHitDetectedMissions) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.RelationshipsHitDetectedMissions = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RelationshipsHitDetectedMissions);
            Misc.SetDirty();

            // 文字色を変更する
            relationshipsHitDetectedMissionsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [第三国の諜報活動を報告するか]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowThirdCountrySpyReportsComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.ShowThirdCountrySpyReports;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.ShowThirdCountrySpyReports))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = showThirdCountrySpyReportsComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [第三国の諜報活動を報告するか]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowThirdCountrySpyReportsComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (showThirdCountrySpyReportsComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = showThirdCountrySpyReportsComboBox.SelectedIndex;
            if (val == Misc.Economy.ShowThirdCountrySpyReports)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ShowThirdCountrySpyReports = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ShowThirdCountrySpyReports);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            showThirdCountrySpyReportsComboBox.Refresh();
        }

        /// <summary>
        ///     [諜報任務の近隣国補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDistanceModifierNeighboursTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(distanceModifierNeighboursTextBox.Text, out val))
            {
                distanceModifierNeighboursTextBox.Text =
                    Misc.Economy.DistanceModifierNeighbours.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0 || val > 1)
            {
                distanceModifierNeighboursTextBox.Text = Misc.Economy.DistanceModifierNeighbours.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DistanceModifierNeighbours) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DistanceModifierNeighbours = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DistanceModifierNeighbours);
            Misc.SetDirty();

            // 文字色を変更する
            distanceModifierNeighboursTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [情報の正確さ補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyInformationAccuracyModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spyInformationAccuracyModifierTextBox.Text, out val))
            {
                spyInformationAccuracyModifierTextBox.Text =
                    Misc.Economy.SpyInformationAccuracyModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < -10 || val > 10)
            {
                spyInformationAccuracyModifierTextBox.Text = Misc.Economy.SpyInformationAccuracyModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SpyInformationAccuracyModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SpyInformationAccuracyModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyInformationAccuracyModifier);
            Misc.SetDirty();

            // 文字色を変更する
            spyInformationAccuracyModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [AIの平時の攻撃的諜報活動]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiPeacetimeSpyMissionsComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.AiPeacetimeSpyMissions;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.AiPeacetimeSpyMissions))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = aiPeacetimeSpyMissionsComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [AIの平時の攻撃的諜報活動]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiPeacetimeSpyMissionsComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (aiPeacetimeSpyMissionsComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = aiPeacetimeSpyMissionsComboBox.SelectedIndex;
            if (val == Misc.Economy.AiPeacetimeSpyMissions)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.AiPeacetimeSpyMissions = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.AiPeacetimeSpyMissions);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            aiPeacetimeSpyMissionsComboBox.Refresh();
        }

        /// <summary>
        ///     [諜報コスト補正の最大IC]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxIcCostModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxIcCostModifierTextBox.Text, out val))
            {
                maxIcCostModifierTextBox.Text = Misc.Economy.MaxIcCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val <0)
            {
                maxIcCostModifierTextBox.Text = Misc.Economy.MaxIcCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxIcCostModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxIcCostModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxIcCostModifier);
            Misc.SetDirty();

            // 文字色を変更する
            maxIcCostModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [AIの諜報コスト補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiSpyMissionsCostModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(aiSpyMissionsCostModifierTextBox.Text, out val))
            {
                aiSpyMissionsCostModifierTextBox.Text =
                    Misc.Economy.AiSpyMissionsCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                aiSpyMissionsCostModifierTextBox.Text = Misc.Economy.AiSpyMissionsCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.AiSpyMissionsCostModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.AiSpyMissionsCostModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.AiSpyMissionsCostModifier);
            Misc.SetDirty();

            // 文字色を変更する
            aiSpyMissionsCostModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [AIの外交コスト補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiDiplomacyCostModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(aiDiplomacyCostModifierTextBox.Text, out val))
            {
                aiDiplomacyCostModifierTextBox.Text =
                    Misc.Economy.AiDiplomacyCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                aiDiplomacyCostModifierTextBox.Text = Misc.Economy.AiDiplomacyCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.AiDiplomacyCostModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.AiDiplomacyCostModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.AiDiplomacyCostModifier);
            Misc.SetDirty();

            // 文字色を変更する
            aiDiplomacyCostModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [AIの外交影響度補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiInfluenceModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(aiInfluenceModifierTextBox.Text, out val))
            {
                aiInfluenceModifierTextBox.Text = Misc.Economy.AiInfluenceModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                aiInfluenceModifierTextBox.Text = Misc.Economy.AiInfluenceModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.AiInfluenceModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.AiInfluenceModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.AiInfluenceModifier);
            Misc.SetDirty();

            // 文字色を変更する
            aiInfluenceModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [人的資源によるナショナリズムの補正値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNationalismPerManpowerAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(nationalismPerManpowerAoDTextBox.Text, out val))
            {
                nationalismPerManpowerAoDTextBox.Text =
                    Misc.Economy.NationalismPerManpowerAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                nationalismPerManpowerAoDTextBox.Text = Misc.Economy.NationalismPerManpowerAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.NationalismPerManpowerAoD) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.NationalismPerManpowerAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.NationalismPerManpowerAoD);
            Misc.SetDirty();

            // 文字色を変更する
            nationalismPerManpowerAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [中核プロヴィンス効率上昇時間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCoreProvinceEfficiencyRiseTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(coreProvinceEfficiencyRiseTimeTextBox.Text, out val))
            {
                coreProvinceEfficiencyRiseTimeTextBox.Text =
                    Misc.Economy.CoreProvinceEfficiencyRiseTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                coreProvinceEfficiencyRiseTimeTextBox.Text = Misc.Economy.CoreProvinceEfficiencyRiseTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.CoreProvinceEfficiencyRiseTime)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CoreProvinceEfficiencyRiseTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CoreProvinceEfficiencyRiseTime);
            Misc.SetDirty();

            // 文字色を変更する
            coreProvinceEfficiencyRiseTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [陸軍の物資再備蓄速度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRestockSpeedLandTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(restockSpeedLandTextBox.Text, out val))
            {
                restockSpeedLandTextBox.Text = Misc.Economy.RestockSpeedLand.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                restockSpeedLandTextBox.Text = Misc.Economy.RestockSpeedLand.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.RestockSpeedLand) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.RestockSpeedLand = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RestockSpeedLand);
            Misc.SetDirty();

            // 文字色を変更する
            restockSpeedLandTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [空軍の物資再備蓄速度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRestockSpeedAirTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(restockSpeedAirTextBox.Text, out val))
            {
                restockSpeedAirTextBox.Text = Misc.Economy.RestockSpeedAir.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                restockSpeedAirTextBox.Text = Misc.Economy.RestockSpeedAir.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.RestockSpeedAir) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.RestockSpeedAir = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RestockSpeedAir);
            Misc.SetDirty();

            // 文字色を変更する
            restockSpeedAirTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [海軍の物資再備蓄速度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRestockSpeedNavalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(restockSpeedNavalTextBox.Text, out val))
            {
                restockSpeedNavalTextBox.Text = Misc.Economy.RestockSpeedNaval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                restockSpeedNavalTextBox.Text = Misc.Economy.RestockSpeedNaval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.RestockSpeedNaval) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.RestockSpeedNaval = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RestockSpeedNaval);
            Misc.SetDirty();

            // 文字色を変更する
            restockSpeedNavalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [不満度によるクーデター成功率修正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyCoupDissentModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spyCoupDissentModifierTextBox.Text, out val))
            {
                spyCoupDissentModifierTextBox.Text =
                    Misc.Economy.SpyCoupDissentModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyCoupDissentModifierTextBox.Text = Misc.Economy.SpyCoupDissentModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SpyCoupDissentModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SpyCoupDissentModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyCoupDissentModifier);
            Misc.SetDirty();

            // 文字色を変更する
            spyCoupDissentModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [輸送船団変換係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvoyDutyConversionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(convoyDutyConversionTextBox.Text, out val))
            {
                convoyDutyConversionTextBox.Text =
                    Misc.Economy.ConvoyDutyConversion.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                convoyDutyConversionTextBox.Text = Misc.Economy.ConvoyDutyConversion.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ConvoyDutyConversion) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ConvoyDutyConversion = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ConvoyDutyConversion);
            Misc.SetDirty();

            // 文字色を変更する
            convoyDutyConversionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [護衛船団変換係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEscortDutyConversionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(escortDutyConversionTextBox.Text, out val))
            {
                escortDutyConversionTextBox.Text =
                    Misc.Economy.EscortDutyConversion.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                escortDutyConversionTextBox.Text = Misc.Economy.EscortDutyConversion.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.EscortDutyConversion) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.EscortDutyConversion = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.EscortDutyConversion);
            Misc.SetDirty();

            // 文字色を変更する
            escortDutyConversionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [輸送艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTpMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(tpMaxAttachTextBox.Text, out val))
            {
                tpMaxAttachTextBox.Text = Misc.Economy.TpMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tpMaxAttachTextBox.Text = Misc.Economy.TpMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.TpMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TpMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TpMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            tpMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [潜水艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSsMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(ssMaxAttachTextBox.Text, out val))
            {
                ssMaxAttachTextBox.Text = Misc.Economy.SsMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ssMaxAttachTextBox.Text = Misc.Economy.SsMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.SsMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SsMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SsMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            ssMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [原子力潜水艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSsnMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(ssnMaxAttachTextBox.Text, out val))
            {
                ssnMaxAttachTextBox.Text = Misc.Economy.SsnMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ssnMaxAttachTextBox.Text = Misc.Economy.SsnMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.SsnMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SsnMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SsnMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            ssnMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [駆逐艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDdMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(ddMaxAttachTextBox.Text, out val))
            {
                ddMaxAttachTextBox.Text = Misc.Economy.DdMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ddMaxAttachTextBox.Text = Misc.Economy.DdMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.DdMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DdMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DdMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            ddMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [軽巡洋艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(clMaxAttachTextBox.Text, out val))
            {
                clMaxAttachTextBox.Text = Misc.Economy.ClMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                clMaxAttachTextBox.Text = Misc.Economy.ClMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.ClMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ClMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ClMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            clMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [重巡洋艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCaMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(caMaxAttachTextBox.Text, out val))
            {
                caMaxAttachTextBox.Text = Misc.Economy.CaMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                caMaxAttachTextBox.Text = Misc.Economy.CaMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.CaMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CaMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CaMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            caMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [巡洋戦艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBcMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(bcMaxAttachTextBox.Text, out val))
            {
                bcMaxAttachTextBox.Text = Misc.Economy.BcMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                bcMaxAttachTextBox.Text = Misc.Economy.BcMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.BcMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.BcMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.BcMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            bcMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [戦艦最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBbMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(bbMaxAttachTextBox.Text, out val))
            {
                bbMaxAttachTextBox.Text = Misc.Economy.BbMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                bbMaxAttachTextBox.Text = Misc.Economy.BbMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.BbMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.BbMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.BbMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            bbMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [軽空母最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCvlMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(cvlMaxAttachTextBox.Text, out val))
            {
                cvlMaxAttachTextBox.Text = Misc.Economy.CvlMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                cvlMaxAttachTextBox.Text = Misc.Economy.CvlMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.CvlMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CvlMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CvlMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            cvlMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [空母最大付属装備数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCvMaxAttachTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(cvMaxAttachTextBox.Text, out val))
            {
                cvMaxAttachTextBox.Text = Misc.Economy.CvMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                cvMaxAttachTextBox.Text = Misc.Economy.CvMaxAttach.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.CvMaxAttach)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CvMaxAttach = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CvMaxAttach);
            Misc.SetDirty();

            // 文字色を変更する
            cvMaxAttachTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [プレイヤーの国策変更を許可]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanChangeIdeasComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.CanChangeIdeas ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.CanChangeIdeas))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = canChangeIdeasComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [プレイヤーの国策変更を許可]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanChangeIdeasComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (canChangeIdeasComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (canChangeIdeasComboBox.SelectedIndex == 1);
            if (val == Misc.Economy.CanChangeIdeas)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CanChangeIdeas = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CanChangeIdeas);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            canChangeIdeasComboBox.Refresh();
        }

        #endregion

        #region 経済2タブ

        /// <summary>
        ///     経済2タブの項目を初期化する
        /// </summary>
        private void InitEconomy2Items()
        {
            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                dissentChangeSpeedLabel.Enabled = true;
                gearingResourceIncrementLabel.Enabled = true;
                gearingLossNoIcLabel.Enabled = true;
                costRepairBuildingsLabel.Enabled = true;
                timeRepairBuildingLabel.Enabled = true;
                provinceEfficiencyRiseTimeLabel.Enabled = true;
                lineUpkeepLabel.Enabled = true;
                lineStartupTimeLabel.Enabled = true;
                lineUpgradeTimeLabel.Enabled = true;
                retoolingCostLabel.Enabled = true;
                retoolingResourceLabel.Enabled = true;
                dailyAgingManpowerLabel.Enabled = true;
                supplyConvoyHuntLabel.Enabled = true;
                supplyNavalStaticAoDLabel.Enabled = true;
                supplyNavalMovingLabel.Enabled = true;
                supplyNavalBattleAoDLabel.Enabled = true;
                supplyAirStaticAoDLabel.Enabled = true;
                supplyAirMovingLabel.Enabled = true;
                supplyAirBattleAoDLabel.Enabled = true;
                supplyAirBombingLabel.Enabled = true;
                supplyLandStaticAoDLabel.Enabled = true;
                supplyLandMovingLabel.Enabled = true;
                supplyLandBattleAoDLabel.Enabled = true;
                supplyLandBombingLabel.Enabled = true;
                supplyStockLandLabel.Enabled = true;
                supplyStockAirLabel.Enabled = true;
                supplyStockNavalLabel.Enabled = true;
                syntheticOilConversionMultiplierLabel.Enabled = true;
                syntheticRaresConversionMultiplierLabel.Enabled = true;
                militarySalaryLabel.Enabled = true;
                maxIntelligenceExpenditureLabel.Enabled = true;
                maxResearchExpenditureLabel.Enabled = true;
                militarySalaryAttrictionModifierLabel.Enabled = true;
                militarySalaryDissentModifierLabel.Enabled = true;
                nuclearSiteUpkeepCostLabel.Enabled = true;
                nuclearPowerUpkeepCostLabel.Enabled = true;
                syntheticOilSiteUpkeepCostLabel.Enabled = true;
                syntheticRaresSiteUpkeepCostLabel.Enabled = true;
                durationDetectionLabel.Enabled = true;
                convoyProvinceHostileTimeLabel.Enabled = true;
                convoyProvinceBlockedTimeLabel.Enabled = true;
                autoTradeConvoyLabel.Enabled = true;
                spyUpkeepCostLabel.Enabled = true;
                spyDetectionChanceLabel.Enabled = true;
                infraEfficiencyModifierLabel.Enabled = true;
                manpowerToConsumerGoodsLabel.Enabled = true;
                timeBetweenSliderChangesAoDLabel.Enabled = true;
                minimalPlacementIcLabel.Enabled = true;
                nuclearPowerLabel.Enabled = true;
                freeInfraRepairLabel.Enabled = true;
                maxSliderDissentLabel.Enabled = true;
                minSliderDissentLabel.Enabled = true;
                maxDissentSliderMoveLabel.Enabled = true;
                icConcentrationBonusLabel.Enabled = true;
                transportConversionLabel.Enabled = true;
                ministerChangeDelayLabel.Enabled = true;
                ministerChangeEventDelayLabel.Enabled = true;
                ideaChangeDelayLabel.Enabled = true;
                ideaChangeEventDelayLabel.Enabled = true;
                leaderChangeDelayLabel.Enabled = true;
                changeIdeaDissentLabel.Enabled = true;
                changeMinisterDissentLabel.Enabled = true;
                minDissentRevoltLabel.Enabled = true;
                dissentRevoltMultiplierLabel.Enabled = true;

                dissentChangeSpeedTextBox.Enabled = true;
                gearingResourceIncrementTextBox.Enabled = true;
                gearingLossNoIcTextBox.Enabled = true;
                costRepairBuildingsTextBox.Enabled = true;
                timeRepairBuildingTextBox.Enabled = true;
                provinceEfficiencyRiseTimeTextBox.Enabled = true;
                lineUpkeepTextBox.Enabled = true;
                lineStartupTimeTextBox.Enabled = true;
                lineUpgradeTimeTextBox.Enabled = true;
                retoolingCostTextBox.Enabled = true;
                retoolingResourceTextBox.Enabled = true;
                dailyAgingManpowerTextBox.Enabled = true;
                supplyConvoyHuntTextBox.Enabled = true;
                supplyNavalStaticAoDTextBox.Enabled = true;
                supplyNavalMovingTextBox.Enabled = true;
                supplyNavalBattleAoDTextBox.Enabled = true;
                supplyAirStaticAoDTextBox.Enabled = true;
                supplyAirMovingTextBox.Enabled = true;
                supplyAirBattleAoDTextBox.Enabled = true;
                supplyAirBombingTextBox.Enabled = true;
                supplyLandStaticAoDTextBox.Enabled = true;
                supplyLandMovingTextBox.Enabled = true;
                supplyLandBattleAoDTextBox.Enabled = true;
                supplyLandBombingTextBox.Enabled = true;
                supplyStockLandTextBox.Enabled = true;
                supplyStockAirTextBox.Enabled = true;
                supplyStockNavalTextBox.Enabled = true;
                syntheticOilConversionMultiplierTextBox.Enabled = true;
                syntheticRaresConversionMultiplierTextBox.Enabled = true;
                militarySalaryTextBox.Enabled = true;
                maxIntelligenceExpenditureTextBox.Enabled = true;
                maxResearchExpenditureTextBox.Enabled = true;
                militarySalaryAttrictionModifierTextBox.Enabled = true;
                militarySalaryDissentModifierTextBox.Enabled = true;
                nuclearSiteUpkeepCostTextBox.Enabled = true;
                nuclearPowerUpkeepCostTextBox.Enabled = true;
                syntheticOilSiteUpkeepCostTextBox.Enabled = true;
                syntheticRaresSiteUpkeepCostTextBox.Enabled = true;
                durationDetectionTextBox.Enabled = true;
                convoyProvinceHostileTimeTextBox.Enabled = true;
                convoyProvinceBlockedTimeTextBox.Enabled = true;
                autoTradeConvoyTextBox.Enabled = true;
                spyUpkeepCostTextBox.Enabled = true;
                spyDetectionChanceTextBox.Enabled = true;
                infraEfficiencyModifierTextBox.Enabled = true;
                manpowerToConsumerGoodsTextBox.Enabled = true;
                timeBetweenSliderChangesAoDTextBox.Enabled = true;
                minimalPlacementIcTextBox.Enabled = true;
                nuclearPowerTextBox.Enabled = true;
                freeInfraRepairTextBox.Enabled = true;
                maxSliderDissentTextBox.Enabled = true;
                minSliderDissentTextBox.Enabled = true;
                maxDissentSliderMoveTextBox.Enabled = true;
                icConcentrationBonusTextBox.Enabled = true;
                transportConversionTextBox.Enabled = true;
                ministerChangeDelayTextBox.Enabled = true;
                ministerChangeEventDelayTextBox.Enabled = true;
                ideaChangeDelayTextBox.Enabled = true;
                ideaChangeEventDelayTextBox.Enabled = true;
                leaderChangeDelayTextBox.Enabled = true;
                changeIdeaDissentTextBox.Enabled = true;
                changeMinisterDissentTextBox.Enabled = true;
                minDissentRevoltTextBox.Enabled = true;
                dissentRevoltMultiplierTextBox.Enabled = true;
            }
            else
            {
                dissentChangeSpeedLabel.Enabled = false;
                gearingResourceIncrementLabel.Enabled = false;
                gearingLossNoIcLabel.Enabled = false;
                costRepairBuildingsLabel.Enabled = false;
                timeRepairBuildingLabel.Enabled = false;
                provinceEfficiencyRiseTimeLabel.Enabled = false;
                lineUpkeepLabel.Enabled = false;
                lineStartupTimeLabel.Enabled = false;
                lineUpgradeTimeLabel.Enabled = false;
                retoolingCostLabel.Enabled = false;
                retoolingResourceLabel.Enabled = false;
                dailyAgingManpowerLabel.Enabled = false;
                supplyConvoyHuntLabel.Enabled = false;
                supplyNavalStaticAoDLabel.Enabled = false;
                supplyNavalMovingLabel.Enabled = false;
                supplyNavalBattleAoDLabel.Enabled = false;
                supplyAirStaticAoDLabel.Enabled = false;
                supplyAirMovingLabel.Enabled = false;
                supplyAirBattleAoDLabel.Enabled = false;
                supplyAirBombingLabel.Enabled = false;
                supplyLandStaticAoDLabel.Enabled = false;
                supplyLandMovingLabel.Enabled = false;
                supplyLandBattleAoDLabel.Enabled = false;
                supplyLandBombingLabel.Enabled = false;
                supplyStockLandLabel.Enabled = false;
                supplyStockAirLabel.Enabled = false;
                supplyStockNavalLabel.Enabled = false;
                syntheticOilConversionMultiplierLabel.Enabled = false;
                syntheticRaresConversionMultiplierLabel.Enabled = false;
                militarySalaryLabel.Enabled = false;
                maxIntelligenceExpenditureLabel.Enabled = false;
                maxResearchExpenditureLabel.Enabled = false;
                militarySalaryAttrictionModifierLabel.Enabled = false;
                militarySalaryDissentModifierLabel.Enabled = false;
                nuclearSiteUpkeepCostLabel.Enabled = false;
                nuclearPowerUpkeepCostLabel.Enabled = false;
                syntheticOilSiteUpkeepCostLabel.Enabled = false;
                syntheticRaresSiteUpkeepCostLabel.Enabled = false;
                durationDetectionLabel.Enabled = false;
                convoyProvinceHostileTimeLabel.Enabled = false;
                convoyProvinceBlockedTimeLabel.Enabled = false;
                autoTradeConvoyLabel.Enabled = false;
                spyUpkeepCostLabel.Enabled = false;
                spyDetectionChanceLabel.Enabled = false;
                infraEfficiencyModifierLabel.Enabled = false;
                manpowerToConsumerGoodsLabel.Enabled = false;
                timeBetweenSliderChangesAoDLabel.Enabled = false;
                minimalPlacementIcLabel.Enabled = false;
                nuclearPowerLabel.Enabled = false;
                freeInfraRepairLabel.Enabled = false;
                maxSliderDissentLabel.Enabled = false;
                minSliderDissentLabel.Enabled = false;
                maxDissentSliderMoveLabel.Enabled = false;
                icConcentrationBonusLabel.Enabled = false;
                transportConversionLabel.Enabled = false;
                ministerChangeDelayLabel.Enabled = false;
                ministerChangeEventDelayLabel.Enabled = false;
                ideaChangeDelayLabel.Enabled = false;
                ideaChangeEventDelayLabel.Enabled = false;
                leaderChangeDelayLabel.Enabled = false;
                changeIdeaDissentLabel.Enabled = false;
                changeMinisterDissentLabel.Enabled = false;
                minDissentRevoltLabel.Enabled = false;
                dissentRevoltMultiplierLabel.Enabled = false;

                dissentChangeSpeedTextBox.Enabled = false;
                gearingResourceIncrementTextBox.Enabled = false;
                gearingLossNoIcTextBox.Enabled = false;
                costRepairBuildingsTextBox.Enabled = false;
                timeRepairBuildingTextBox.Enabled = false;
                provinceEfficiencyRiseTimeTextBox.Enabled = false;
                lineUpkeepTextBox.Enabled = false;
                lineStartupTimeTextBox.Enabled = false;
                lineUpgradeTimeTextBox.Enabled = false;
                retoolingCostTextBox.Enabled = false;
                retoolingResourceTextBox.Enabled = false;
                dailyAgingManpowerTextBox.Enabled = false;
                supplyConvoyHuntTextBox.Enabled = false;
                supplyNavalStaticAoDTextBox.Enabled = false;
                supplyNavalMovingTextBox.Enabled = false;
                supplyNavalBattleAoDTextBox.Enabled = false;
                supplyAirStaticAoDTextBox.Enabled = false;
                supplyAirMovingTextBox.Enabled = false;
                supplyAirBattleAoDTextBox.Enabled = false;
                supplyAirBombingTextBox.Enabled = false;
                supplyLandStaticAoDTextBox.Enabled = false;
                supplyLandMovingTextBox.Enabled = false;
                supplyLandBattleAoDTextBox.Enabled = false;
                supplyLandBombingTextBox.Enabled = false;
                supplyStockLandTextBox.Enabled = false;
                supplyStockAirTextBox.Enabled = false;
                supplyStockNavalTextBox.Enabled = false;
                syntheticOilConversionMultiplierTextBox.Enabled = false;
                syntheticRaresConversionMultiplierTextBox.Enabled = false;
                militarySalaryTextBox.Enabled = false;
                maxIntelligenceExpenditureTextBox.Enabled = false;
                maxResearchExpenditureTextBox.Enabled = false;
                militarySalaryAttrictionModifierTextBox.Enabled = false;
                militarySalaryDissentModifierTextBox.Enabled = false;
                nuclearSiteUpkeepCostTextBox.Enabled = false;
                nuclearPowerUpkeepCostTextBox.Enabled = false;
                syntheticOilSiteUpkeepCostTextBox.Enabled = false;
                syntheticRaresSiteUpkeepCostTextBox.Enabled = false;
                durationDetectionTextBox.Enabled = false;
                convoyProvinceHostileTimeTextBox.Enabled = false;
                convoyProvinceBlockedTimeTextBox.Enabled = false;
                autoTradeConvoyTextBox.Enabled = false;
                spyUpkeepCostTextBox.Enabled = false;
                spyDetectionChanceTextBox.Enabled = false;
                infraEfficiencyModifierTextBox.Enabled = false;
                manpowerToConsumerGoodsTextBox.Enabled = false;
                timeBetweenSliderChangesAoDTextBox.Enabled = false;
                minimalPlacementIcTextBox.Enabled = false;
                nuclearPowerTextBox.Enabled = false;
                freeInfraRepairTextBox.Enabled = false;
                maxSliderDissentTextBox.Enabled = false;
                minSliderDissentTextBox.Enabled = false;
                maxDissentSliderMoveTextBox.Enabled = false;
                icConcentrationBonusTextBox.Enabled = false;
                transportConversionTextBox.Enabled = false;
                ministerChangeDelayTextBox.Enabled = false;
                ministerChangeEventDelayTextBox.Enabled = false;
                ideaChangeDelayTextBox.Enabled = false;
                ideaChangeEventDelayTextBox.Enabled = false;
                leaderChangeDelayTextBox.Enabled = false;
                changeIdeaDissentTextBox.Enabled = false;
                changeMinisterDissentTextBox.Enabled = false;
                minDissentRevoltTextBox.Enabled = false;
                dissentRevoltMultiplierTextBox.Enabled = false;
            }
        }

        /// <summary>
        ///     経済2タブの項目を更新する
        /// </summary>
        private void UpdateEconomy2Items()
        {
            // 編集項目の値を更新する
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                dissentChangeSpeedTextBox.Text = Misc.Economy.DissentChangeSpeed.ToString(CultureInfo.InvariantCulture);
                gearingResourceIncrementTextBox.Text = Misc.Economy.GearingResourceIncrement.ToString(CultureInfo.InvariantCulture);
                gearingLossNoIcTextBox.Text = Misc.Economy.GearingLossNoIc.ToString(CultureInfo.InvariantCulture);
                costRepairBuildingsTextBox.Text = Misc.Economy.CostRepairBuildings.ToString(CultureInfo.InvariantCulture);
                timeRepairBuildingTextBox.Text = Misc.Economy.TimeRepairBuilding.ToString(CultureInfo.InvariantCulture);
                provinceEfficiencyRiseTimeTextBox.Text = Misc.Economy.ProvinceEfficiencyRiseTime.ToString(CultureInfo.InvariantCulture);
                lineUpkeepTextBox.Text = Misc.Economy.LineUpkeep.ToString(CultureInfo.InvariantCulture);
                lineStartupTimeTextBox.Text = Misc.Economy.LineStartupTime.ToString(CultureInfo.InvariantCulture);
                lineUpgradeTimeTextBox.Text = Misc.Economy.LineUpgradeTime.ToString(CultureInfo.InvariantCulture);
                retoolingCostTextBox.Text = Misc.Economy.RetoolingCost.ToString(CultureInfo.InvariantCulture);
                retoolingResourceTextBox.Text = Misc.Economy.RetoolingResource.ToString(CultureInfo.InvariantCulture);
                dailyAgingManpowerTextBox.Text = Misc.Economy.DailyAgingManpower.ToString(CultureInfo.InvariantCulture);
                supplyConvoyHuntTextBox.Text = Misc.Economy.SupplyConvoyHunt.ToString(CultureInfo.InvariantCulture);
                supplyNavalStaticAoDTextBox.Text = Misc.Economy.SupplyNavalStaticAoD.ToString(CultureInfo.InvariantCulture);
                supplyNavalMovingTextBox.Text = Misc.Economy.SupplyNavalMoving.ToString(CultureInfo.InvariantCulture);
                supplyNavalBattleAoDTextBox.Text = Misc.Economy.SupplyNavalBattleAoD.ToString(CultureInfo.InvariantCulture);
                supplyAirStaticAoDTextBox.Text = Misc.Economy.SupplyAirStaticAoD.ToString(CultureInfo.InvariantCulture);
                supplyAirMovingTextBox.Text = Misc.Economy.SupplyAirMoving.ToString(CultureInfo.InvariantCulture);
                supplyAirBattleAoDTextBox.Text = Misc.Economy.SupplyAirBattleAoD.ToString(CultureInfo.InvariantCulture);
                supplyAirBombingTextBox.Text = Misc.Economy.SupplyAirBombing.ToString(CultureInfo.InvariantCulture);
                supplyLandStaticAoDTextBox.Text = Misc.Economy.SupplyLandStaticAoD.ToString(CultureInfo.InvariantCulture);
                supplyLandMovingTextBox.Text = Misc.Economy.SupplyLandMoving.ToString(CultureInfo.InvariantCulture);
                supplyLandBattleAoDTextBox.Text = Misc.Economy.SupplyLandBattleAoD.ToString(CultureInfo.InvariantCulture);
                supplyLandBombingTextBox.Text = Misc.Economy.SupplyLandBombing.ToString(CultureInfo.InvariantCulture);
                supplyStockLandTextBox.Text = Misc.Economy.SupplyStockLand.ToString(CultureInfo.InvariantCulture);
                supplyStockAirTextBox.Text = Misc.Economy.SupplyStockAir.ToString(CultureInfo.InvariantCulture);
                supplyStockNavalTextBox.Text = Misc.Economy.SupplyStockNaval.ToString(CultureInfo.InvariantCulture);
                syntheticOilConversionMultiplierTextBox.Text = Misc.Economy.SyntheticOilConversionMultiplier.ToString(CultureInfo.InvariantCulture);
                syntheticRaresConversionMultiplierTextBox.Text = Misc.Economy.SyntheticRaresConversionMultiplier.ToString(CultureInfo.InvariantCulture);
                militarySalaryTextBox.Text = Misc.Economy.MilitarySalary.ToString(CultureInfo.InvariantCulture);
                maxIntelligenceExpenditureTextBox.Text = Misc.Economy.MaxIntelligenceExpenditure.ToString(CultureInfo.InvariantCulture);
                maxResearchExpenditureTextBox.Text = Misc.Economy.MaxResearchExpenditure.ToString(CultureInfo.InvariantCulture);
                militarySalaryAttrictionModifierTextBox.Text = Misc.Economy.MilitarySalaryAttrictionModifier.ToString(CultureInfo.InvariantCulture);
                militarySalaryDissentModifierTextBox.Text = Misc.Economy.MilitarySalaryDissentModifier.ToString(CultureInfo.InvariantCulture);
                nuclearSiteUpkeepCostTextBox.Text = Misc.Economy.NuclearSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                nuclearPowerUpkeepCostTextBox.Text = Misc.Economy.NuclearPowerUpkeepCost.ToString(CultureInfo.InvariantCulture);
                syntheticOilSiteUpkeepCostTextBox.Text = Misc.Economy.SyntheticOilSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                syntheticRaresSiteUpkeepCostTextBox.Text = Misc.Economy.SyntheticRaresSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                durationDetectionTextBox.Text = Misc.Economy.DurationDetection.ToString(CultureInfo.InvariantCulture);
                convoyProvinceHostileTimeTextBox.Text = Misc.Economy.ConvoyProvinceHostileTime.ToString(CultureInfo.InvariantCulture);
                convoyProvinceBlockedTimeTextBox.Text = Misc.Economy.ConvoyProvinceBlockedTime.ToString(CultureInfo.InvariantCulture);
                autoTradeConvoyTextBox.Text = Misc.Economy.AutoTradeConvoy.ToString(CultureInfo.InvariantCulture);
                spyUpkeepCostTextBox.Text = Misc.Economy.SpyUpkeepCost.ToString(CultureInfo.InvariantCulture);
                spyDetectionChanceTextBox.Text = Misc.Economy.SpyDetectionChance.ToString(CultureInfo.InvariantCulture);
                infraEfficiencyModifierTextBox.Text = Misc.Economy.InfraEfficiencyModifier.ToString(CultureInfo.InvariantCulture);
                manpowerToConsumerGoodsTextBox.Text = Misc.Economy.ManpowerToConsumerGoods.ToString(CultureInfo.InvariantCulture);
                timeBetweenSliderChangesAoDTextBox.Text = Misc.Economy.TimeBetweenSliderChangesAoD.ToString(CultureInfo.InvariantCulture);
                minimalPlacementIcTextBox.Text = Misc.Economy.MinimalPlacementIc.ToString(CultureInfo.InvariantCulture);
                nuclearPowerTextBox.Text = Misc.Economy.NuclearPower.ToString(CultureInfo.InvariantCulture);
                freeInfraRepairTextBox.Text = Misc.Economy.FreeInfraRepair.ToString(CultureInfo.InvariantCulture);
                maxSliderDissentTextBox.Text = Misc.Economy.MaxSliderDissent.ToString(CultureInfo.InvariantCulture);
                minSliderDissentTextBox.Text = Misc.Economy.MinSliderDissent.ToString(CultureInfo.InvariantCulture);
                maxDissentSliderMoveTextBox.Text = Misc.Economy.MaxDissentSliderMove.ToString(CultureInfo.InvariantCulture);
                icConcentrationBonusTextBox.Text = Misc.Economy.IcConcentrationBonus.ToString(CultureInfo.InvariantCulture);
                transportConversionTextBox.Text = Misc.Economy.TransportConversion.ToString(CultureInfo.InvariantCulture);
                ministerChangeDelayTextBox.Text = Misc.Economy.MinisterChangeDelay.ToString(CultureInfo.InvariantCulture);
                ministerChangeEventDelayTextBox.Text = Misc.Economy.MinisterChangeEventDelay.ToString(CultureInfo.InvariantCulture);
                ideaChangeDelayTextBox.Text = Misc.Economy.IdeaChangeDelay.ToString(CultureInfo.InvariantCulture);
                ideaChangeEventDelayTextBox.Text = Misc.Economy.IdeaChangeEventDelay.ToString(CultureInfo.InvariantCulture);
                leaderChangeDelayTextBox.Text = Misc.Economy.LeaderChangeDelay.ToString(CultureInfo.InvariantCulture);
                changeIdeaDissentTextBox.Text = Misc.Economy.ChangeIdeaDissent.ToString(CultureInfo.InvariantCulture);
                changeMinisterDissentTextBox.Text = Misc.Economy.ChangeMinisterDissent.ToString(CultureInfo.InvariantCulture);
                minDissentRevoltTextBox.Text = Misc.Economy.MinDissentRevolt.ToString(CultureInfo.InvariantCulture);
                dissentRevoltMultiplierTextBox.Text = Misc.Economy.DissentRevoltMultiplier.ToString(CultureInfo.InvariantCulture);
            }

            // 編集項目の色を更新する
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                dissentChangeSpeedTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentChangeSpeed) ? Color.Red : SystemColors.WindowText;
                gearingResourceIncrementTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingResourceIncrement) ? Color.Red : SystemColors.WindowText;
                gearingLossNoIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingLossNoIc) ? Color.Red : SystemColors.WindowText;
                costRepairBuildingsTextBox.ForeColor = Misc.IsDirty(MiscItemId.CostRepairBuildings) ? Color.Red : SystemColors.WindowText;
                timeRepairBuildingTextBox.ForeColor = Misc.IsDirty(MiscItemId.TimeRepairBuilding) ? Color.Red : SystemColors.WindowText;
                provinceEfficiencyRiseTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ProvinceEfficiencyRiseTime) ? Color.Red : SystemColors.WindowText;
                lineUpkeepTextBox.ForeColor = Misc.IsDirty(MiscItemId.LineUpkeep) ? Color.Red : SystemColors.WindowText;
                lineStartupTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.LineStartupTime) ? Color.Red : SystemColors.WindowText;
                lineUpgradeTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.LineUpgradeTime) ? Color.Red : SystemColors.WindowText;
                retoolingCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.RetoolingCost) ? Color.Red : SystemColors.WindowText;
                retoolingResourceTextBox.ForeColor = Misc.IsDirty(MiscItemId.RetoolingResource) ? Color.Red : SystemColors.WindowText;
                dailyAgingManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.DailyAgingManpower) ? Color.Red : SystemColors.WindowText;
                supplyConvoyHuntTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyConvoyHunt) ? Color.Red : SystemColors.WindowText;
                supplyNavalStaticAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalStaticAoD) ? Color.Red : SystemColors.WindowText;
                supplyNavalMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalMoving) ? Color.Red : SystemColors.WindowText;
                supplyNavalBattleAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalBattleAoD) ? Color.Red : SystemColors.WindowText;
                supplyAirStaticAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirStaticAoD) ? Color.Red : SystemColors.WindowText;
                supplyAirMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirMoving) ? Color.Red : SystemColors.WindowText;
                supplyAirBattleAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirBattleAoD) ? Color.Red : SystemColors.WindowText;
                supplyAirBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirBombing) ? Color.Red : SystemColors.WindowText;
                supplyLandStaticAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandStaticAoD) ? Color.Red : SystemColors.WindowText;
                supplyLandMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandMoving) ? Color.Red : SystemColors.WindowText;
                supplyLandBattleAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandBattleAoD) ? Color.Red : SystemColors.WindowText;
                supplyLandBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandBombing) ? Color.Red : SystemColors.WindowText;
                supplyStockLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyStockLand) ? Color.Red : SystemColors.WindowText;
                supplyStockAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyStockAir) ? Color.Red : SystemColors.WindowText;
                supplyStockNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyStockNaval) ? Color.Red : SystemColors.WindowText;
                syntheticOilConversionMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SyntheticOilConversionMultiplier) ? Color.Red : SystemColors.WindowText;
                syntheticRaresConversionMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SyntheticRaresConversionMultiplier) ? Color.Red : SystemColors.WindowText;
                militarySalaryTextBox.ForeColor = Misc.IsDirty(MiscItemId.MilitarySalary) ? Color.Red : SystemColors.WindowText;
                maxIntelligenceExpenditureTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxIntelligenceExpenditure) ? Color.Red : SystemColors.WindowText;
                maxResearchExpenditureTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxResearchExpenditure) ? Color.Red : SystemColors.WindowText;
                militarySalaryAttrictionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MilitarySalaryAttrictionModifier) ? Color.Red : SystemColors.WindowText;
                militarySalaryDissentModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MilitarySalaryDissentModifier) ? Color.Red : SystemColors.WindowText;
                nuclearSiteUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.NuclearSiteUpkeepCost) ? Color.Red : SystemColors.WindowText;
                nuclearPowerUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.NuclearPowerUpkeepCost) ? Color.Red : SystemColors.WindowText;
                syntheticOilSiteUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.SyntheticOilSiteUpkeepCost) ? Color.Red : SystemColors.WindowText;
                syntheticRaresSiteUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.SyntheticRaresSiteUpkeepCost) ? Color.Red : SystemColors.WindowText;
                durationDetectionTextBox.ForeColor = Misc.IsDirty(MiscItemId.DurationDetection) ? Color.Red : SystemColors.WindowText;
                convoyProvinceHostileTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyProvinceHostileTime) ? Color.Red : SystemColors.WindowText;
                convoyProvinceBlockedTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyProvinceBlockedTime) ? Color.Red : SystemColors.WindowText;
                autoTradeConvoyTextBox.ForeColor = Misc.IsDirty(MiscItemId.AutoTradeConvoy) ? Color.Red : SystemColors.WindowText;
                spyUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyUpkeepCost) ? Color.Red : SystemColors.WindowText;
                spyDetectionChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyDetectionChance) ? Color.Red : SystemColors.WindowText;
                infraEfficiencyModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.InfraEfficiencyModifier) ? Color.Red : SystemColors.WindowText;
                manpowerToConsumerGoodsTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerToConsumerGoods) ? Color.Red : SystemColors.WindowText;
                timeBetweenSliderChangesAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.TimeBetweenSliderChangesAoD) ? Color.Red : SystemColors.WindowText;
                minimalPlacementIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinimalPlacementIc) ? Color.Red : SystemColors.WindowText;
                nuclearPowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.NuclearPower) ? Color.Red : SystemColors.WindowText;
                freeInfraRepairTextBox.ForeColor = Misc.IsDirty(MiscItemId.FreeInfraRepair) ? Color.Red : SystemColors.WindowText;
                maxSliderDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxSliderDissent) ? Color.Red : SystemColors.WindowText;
                minSliderDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinSliderDissent) ? Color.Red : SystemColors.WindowText;
                maxDissentSliderMoveTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxDissentSliderMove) ? Color.Red : SystemColors.WindowText;
                icConcentrationBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcConcentrationBonus) ? Color.Red : SystemColors.WindowText;
                transportConversionTextBox.ForeColor = Misc.IsDirty(MiscItemId.TransportConversion) ? Color.Red : SystemColors.WindowText;
                ministerChangeDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinisterChangeDelay) ? Color.Red : SystemColors.WindowText;
                ministerChangeEventDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinisterChangeEventDelay) ? Color.Red : SystemColors.WindowText;
                ideaChangeDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.IdeaChangeDelay) ? Color.Red : SystemColors.WindowText;
                ideaChangeEventDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.IdeaChangeEventDelay) ? Color.Red : SystemColors.WindowText;
                leaderChangeDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.LeaderChangeDelay) ? Color.Red : SystemColors.WindowText;
                changeIdeaDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChangeIdeaDissent) ? Color.Red : SystemColors.WindowText;
                changeMinisterDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChangeMinisterDissent) ? Color.Red : SystemColors.WindowText;
                minDissentRevoltTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinDissentRevolt) ? Color.Red : SystemColors.WindowText;
                dissentRevoltMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentRevoltMultiplier) ? Color.Red : SystemColors.WindowText;
            }
        }

        /// <summary>
        /// [不満度変化速度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDissentChangeSpeedTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(dissentChangeSpeedTextBox.Text, out val))
            {
                dissentChangeSpeedTextBox.Text = Misc.Economy.DissentChangeSpeed.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                dissentChangeSpeedTextBox.Text = Misc.Economy.DissentChangeSpeed.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DissentChangeSpeed) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DissentChangeSpeed = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DissentChangeSpeed);
            Misc.SetDirty();

            // 文字色を変更する
            dissentChangeSpeedTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [連続生産時の資源消費増加]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGearingResourceIncrementTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(gearingResourceIncrementTextBox.Text, out val))
            {
                gearingResourceIncrementTextBox.Text = Misc.Economy.GearingResourceIncrement.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                gearingResourceIncrementTextBox.Text = Misc.Economy.GearingResourceIncrement.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.GearingResourceIncrement) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.GearingResourceIncrement = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.GearingResourceIncrement);
            Misc.SetDirty();

            // 文字色を変更する
            gearingResourceIncrementTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [IC不足時のギアリングボーナス減少値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGearingLossNoIcTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(gearingLossNoIcTextBox.Text, out val))
            {
                gearingLossNoIcTextBox.Text = Misc.Economy.GearingLossNoIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                gearingLossNoIcTextBox.Text = Misc.Economy.GearingLossNoIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.GearingLossNoIc) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.GearingLossNoIc = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.GearingLossNoIc);
            Misc.SetDirty();

            // 文字色を変更する
            gearingLossNoIcTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [建物修復コスト補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCostRepairBuildingsTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(costRepairBuildingsTextBox.Text, out val))
            {
                costRepairBuildingsTextBox.Text = Misc.Economy.CostRepairBuildings.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                costRepairBuildingsTextBox.Text = Misc.Economy.CostRepairBuildings.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.CostRepairBuildings) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CostRepairBuildings = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CostRepairBuildings);
            Misc.SetDirty();

            // 文字色を変更する
            costRepairBuildingsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [建物修復時間補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeRepairBuildingTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(timeRepairBuildingTextBox.Text, out val))
            {
                timeRepairBuildingTextBox.Text = Misc.Economy.TimeRepairBuilding.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                timeRepairBuildingTextBox.Text = Misc.Economy.TimeRepairBuilding.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TimeRepairBuilding) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TimeRepairBuilding = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TimeRepairBuilding);
            Misc.SetDirty();

            // 文字色を変更する
            timeRepairBuildingTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [プロヴィンス効率上昇時間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceEfficiencyRiseTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(provinceEfficiencyRiseTimeTextBox.Text, out val))
            {
                provinceEfficiencyRiseTimeTextBox.Text = Misc.Economy.ProvinceEfficiencyRiseTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                provinceEfficiencyRiseTimeTextBox.Text = Misc.Economy.ProvinceEfficiencyRiseTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.ProvinceEfficiencyRiseTime)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ProvinceEfficiencyRiseTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ProvinceEfficiencyRiseTime);
            Misc.SetDirty();

            // 文字色を変更する
            provinceEfficiencyRiseTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [ライン維持コスト補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLineUpkeepTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(lineUpkeepTextBox.Text, out val))
            {
                lineUpkeepTextBox.Text = Misc.Economy.LineUpkeep.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                lineUpkeepTextBox.Text = Misc.Economy.LineUpkeep.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.LineUpkeep) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.LineUpkeep = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.LineUpkeep);
            Misc.SetDirty();

            // 文字色を変更する
            lineUpkeepTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [ライン開始時間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLineStartupTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(lineStartupTimeTextBox.Text, out val))
            {
                lineStartupTimeTextBox.Text = Misc.Economy.LineStartupTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                lineStartupTimeTextBox.Text = Misc.Economy.LineStartupTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.LineStartupTime)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.LineStartupTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.LineStartupTime);
            Misc.SetDirty();

            // 文字色を変更する
            lineStartupTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [ライン改良時間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLineUpgradeTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(lineUpgradeTimeTextBox.Text, out val))
            {
                lineUpgradeTimeTextBox.Text = Misc.Economy.LineUpgradeTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                lineUpgradeTimeTextBox.Text = Misc.Economy.LineUpgradeTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.LineUpgradeTime)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.LineUpgradeTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.LineUpgradeTime);
            Misc.SetDirty();

            // 文字色を変更する
            lineUpgradeTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [ライン調整コスト補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetoolingCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(retoolingCostTextBox.Text, out val))
            {
                retoolingCostTextBox.Text = Misc.Economy.RetoolingCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                retoolingCostTextBox.Text = Misc.Economy.RetoolingCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.RetoolingCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.RetoolingCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RetoolingCost);
            Misc.SetDirty();

            // 文字色を変更する
            retoolingCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [ライン調整資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetoolingResourceTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(retoolingResourceTextBox.Text, out val))
            {
                retoolingResourceTextBox.Text = Misc.Economy.RetoolingResource.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                retoolingResourceTextBox.Text = Misc.Economy.RetoolingResource.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.RetoolingResource) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.RetoolingResource = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RetoolingResource);
            Misc.SetDirty();

            // 文字色を変更する
            retoolingResourceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [人的資源老化補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDailyAgingManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(dailyAgingManpowerTextBox.Text, out val))
            {
                dailyAgingManpowerTextBox.Text = Misc.Economy.DailyAgingManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                dailyAgingManpowerTextBox.Text = Misc.Economy.DailyAgingManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DailyAgingManpower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DailyAgingManpower = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DailyAgingManpower);
            Misc.SetDirty();

            // 文字色を変更する
            dailyAgingManpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [船団襲撃時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyConvoyHuntTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyConvoyHuntTextBox.Text, out val))
            {
                supplyConvoyHuntTextBox.Text = Misc.Economy.SupplyConvoyHunt.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyConvoyHuntTextBox.Text = Misc.Economy.SupplyConvoyHunt.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyConvoyHunt) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyConvoyHunt = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyConvoyHunt);
            Misc.SetDirty();

            // 文字色を変更する
            supplyConvoyHuntTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の待機時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyNavalStaticAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyNavalStaticAoDTextBox.Text, out val))
            {
                supplyNavalStaticAoDTextBox.Text = Misc.Economy.SupplyNavalStaticAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyNavalStaticAoDTextBox.Text = Misc.Economy.SupplyNavalStaticAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyNavalStaticAoD) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyNavalStaticAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyNavalStaticAoD);
            Misc.SetDirty();

            // 文字色を変更する
            supplyNavalStaticAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の移動時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyNavalMovingTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyNavalMovingTextBox.Text, out val))
            {
                supplyNavalMovingTextBox.Text = Misc.Economy.SupplyNavalMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyNavalMovingTextBox.Text = Misc.Economy.SupplyNavalMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyNavalMoving) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyNavalMoving = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyNavalMoving);
            Misc.SetDirty();

            // 文字色を変更する
            supplyNavalMovingTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の戦闘時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyNavalBattleAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyNavalBattleAoDTextBox.Text, out val))
            {
                supplyNavalBattleAoDTextBox.Text = Misc.Economy.SupplyNavalBattleAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyNavalBattleAoDTextBox.Text = Misc.Economy.SupplyNavalBattleAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyNavalBattleAoD) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyNavalBattleAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyNavalBattleAoD);
            Misc.SetDirty();

            // 文字色を変更する
            supplyNavalBattleAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の待機時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyAirStaticAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyAirStaticAoDTextBox.Text, out val))
            {
                supplyAirStaticAoDTextBox.Text = Misc.Economy.SupplyAirStaticAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyAirStaticAoDTextBox.Text = Misc.Economy.SupplyAirStaticAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyAirStaticAoD) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyAirStaticAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyAirStaticAoD);
            Misc.SetDirty();

            // 文字色を変更する
            supplyAirStaticAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の移動時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyAirMovingTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyAirMovingTextBox.Text, out val))
            {
                supplyAirMovingTextBox.Text = Misc.Economy.SupplyAirMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyAirMovingTextBox.Text = Misc.Economy.SupplyAirMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyAirMoving) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyAirMoving = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyAirMoving);
            Misc.SetDirty();

            // 文字色を変更する
            supplyAirMovingTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の戦闘時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyAirBattleAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyAirBattleAoDTextBox.Text, out val))
            {
                supplyAirBattleAoDTextBox.Text = Misc.Economy.SupplyAirBattleAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyAirBattleAoDTextBox.Text = Misc.Economy.SupplyAirBattleAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyAirBattleAoD) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyAirBattleAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyAirBattleAoD);
            Misc.SetDirty();

            // 文字色を変更する
            supplyAirBattleAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の爆撃時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyAirBombingTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyAirBombingTextBox.Text, out val))
            {
                supplyAirBombingTextBox.Text = Misc.Economy.SupplyAirBombing.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyAirBombingTextBox.Text = Misc.Economy.SupplyAirBombing.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyAirBombing) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyAirBombing = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyAirBombing);
            Misc.SetDirty();

            // 文字色を変更する
            supplyAirBombingTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の待機時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyLandStaticAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyLandStaticAoDTextBox.Text, out val))
            {
                supplyLandStaticAoDTextBox.Text = Misc.Economy.SupplyLandStaticAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyLandStaticAoDTextBox.Text = Misc.Economy.SupplyLandStaticAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyLandStaticAoD) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyLandStaticAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyLandStaticAoD);
            Misc.SetDirty();

            // 文字色を変更する
            supplyLandStaticAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の移動時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyLandMovingTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyLandMovingTextBox.Text, out val))
            {
                supplyLandMovingTextBox.Text = Misc.Economy.SupplyLandMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyLandMovingTextBox.Text = Misc.Economy.SupplyLandMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyLandMoving) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyLandMoving = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyLandMoving);
            Misc.SetDirty();

            // 文字色を変更する
            supplyLandMovingTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の戦闘時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyLandBattleAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyLandBattleAoDTextBox.Text, out val))
            {
                supplyLandBattleAoDTextBox.Text = Misc.Economy.SupplyLandBattleAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyLandBattleAoDTextBox.Text = Misc.Economy.SupplyLandBattleAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyLandBattleAoD) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyLandBattleAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyLandBattleAoD);
            Misc.SetDirty();

            // 文字色を変更する
            supplyLandBattleAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の砲撃時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyLandBombingTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyLandBombingTextBox.Text, out val))
            {
                supplyLandBombingTextBox.Text = Misc.Economy.SupplyLandBombing.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyLandBombingTextBox.Text = Misc.Economy.SupplyLandBombing.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyLandBombing) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyLandBombing = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyLandBombing);
            Misc.SetDirty();

            // 文字色を変更する
            supplyLandBombingTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の物資備蓄量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyStockLandTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyStockLandTextBox.Text, out val))
            {
                supplyStockLandTextBox.Text = Misc.Economy.SupplyStockLand.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyStockLandTextBox.Text = Misc.Economy.SupplyStockLand.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyStockLand) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyStockLand = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyStockLand);
            Misc.SetDirty();

            // 文字色を変更する
            supplyStockLandTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の物資備蓄量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyStockAirTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyStockAirTextBox.Text, out val))
            {
                supplyStockAirTextBox.Text = Misc.Economy.SupplyStockAir.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyStockAirTextBox.Text = Misc.Economy.SupplyStockAir.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyStockAir) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyStockAir = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyStockAir);
            Misc.SetDirty();

            // 文字色を変更する
            supplyStockAirTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の物資備蓄量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyStockNavalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyStockNavalTextBox.Text, out val))
            {
                supplyStockNavalTextBox.Text = Misc.Economy.SupplyStockNaval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyStockNavalTextBox.Text = Misc.Economy.SupplyStockNaval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyStockNaval) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyStockNaval = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyStockNaval);
            Misc.SetDirty();

            // 文字色を変更する
            supplyStockNavalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [合成石油変換係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSyntheticOilConversionMultiplierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(syntheticOilConversionMultiplierTextBox.Text, out val))
            {
                syntheticOilConversionMultiplierTextBox.Text = Misc.Economy.SyntheticOilConversionMultiplier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                syntheticOilConversionMultiplierTextBox.Text = Misc.Economy.SyntheticOilConversionMultiplier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SyntheticOilConversionMultiplier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SyntheticOilConversionMultiplier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SyntheticOilConversionMultiplier);
            Misc.SetDirty();

            // 文字色を変更する
            syntheticOilConversionMultiplierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [合成希少資源変換係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSyntheticRaresConversionMultiplierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(syntheticRaresConversionMultiplierTextBox.Text, out val))
            {
                syntheticRaresConversionMultiplierTextBox.Text = Misc.Economy.SyntheticRaresConversionMultiplier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                syntheticRaresConversionMultiplierTextBox.Text = Misc.Economy.SyntheticRaresConversionMultiplier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SyntheticRaresConversionMultiplier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SyntheticRaresConversionMultiplier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SyntheticRaresConversionMultiplier);
            Misc.SetDirty();

            // 文字色を変更する
            syntheticRaresConversionMultiplierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [軍隊の給料]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMilitarySalaryTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(militarySalaryTextBox.Text, out val))
            {
                militarySalaryTextBox.Text = Misc.Economy.MilitarySalary.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                militarySalaryTextBox.Text = Misc.Economy.MilitarySalary.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MilitarySalary) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MilitarySalary = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MilitarySalary);
            Misc.SetDirty();

            // 文字色を変更する
            militarySalaryTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [最大諜報費比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxIntelligenceExpenditureTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxIntelligenceExpenditureTextBox.Text, out val))
            {
                maxIntelligenceExpenditureTextBox.Text = Misc.Economy.MaxIntelligenceExpenditure.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxIntelligenceExpenditureTextBox.Text = Misc.Economy.MaxIntelligenceExpenditure.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxIntelligenceExpenditure) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxIntelligenceExpenditure = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxIntelligenceExpenditure);
            Misc.SetDirty();

            // 文字色を変更する
            maxIntelligenceExpenditureTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [最大研究費比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxResearchExpenditureTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxResearchExpenditureTextBox.Text, out val))
            {
                maxResearchExpenditureTextBox.Text = Misc.Economy.MaxResearchExpenditure.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxResearchExpenditureTextBox.Text = Misc.Economy.MaxResearchExpenditure.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxResearchExpenditure) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxResearchExpenditure = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxResearchExpenditure);
            Misc.SetDirty();

            // 文字色を変更する
            maxResearchExpenditureTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [軍隊の給料不足時の消耗補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMilitarySalaryAttrictionModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(militarySalaryAttrictionModifierTextBox.Text, out val))
            {
                militarySalaryAttrictionModifierTextBox.Text = Misc.Economy.MilitarySalaryAttrictionModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                militarySalaryAttrictionModifierTextBox.Text = Misc.Economy.MilitarySalaryAttrictionModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MilitarySalaryAttrictionModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MilitarySalaryAttrictionModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MilitarySalaryAttrictionModifier);
            Misc.SetDirty();

            // 文字色を変更する
            militarySalaryAttrictionModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [軍隊の給料不足時の不満度補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMilitarySalaryDissentModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(militarySalaryDissentModifierTextBox.Text, out val))
            {
                militarySalaryDissentModifierTextBox.Text = Misc.Economy.MilitarySalaryDissentModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                militarySalaryDissentModifierTextBox.Text = Misc.Economy.MilitarySalaryDissentModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MilitarySalaryDissentModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MilitarySalaryDissentModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MilitarySalaryDissentModifier);
            Misc.SetDirty();

            // 文字色を変更する
            militarySalaryDissentModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [原子炉維持コスト]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNuclearSiteUpkeepCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(nuclearSiteUpkeepCostTextBox.Text, out val))
            {
                nuclearSiteUpkeepCostTextBox.Text = Misc.Economy.NuclearSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                nuclearSiteUpkeepCostTextBox.Text = Misc.Economy.NuclearSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.NuclearSiteUpkeepCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.NuclearSiteUpkeepCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.NuclearSiteUpkeepCost);
            Misc.SetDirty();

            // 文字色を変更する
            nuclearSiteUpkeepCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [原子力発電所維持コスト]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNuclearPowerUpkeepCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(nuclearPowerUpkeepCostTextBox.Text, out val))
            {
                nuclearPowerUpkeepCostTextBox.Text = Misc.Economy.NuclearPowerUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                nuclearPowerUpkeepCostTextBox.Text = Misc.Economy.NuclearPowerUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.NuclearPowerUpkeepCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.NuclearPowerUpkeepCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.NuclearPowerUpkeepCost);
            Misc.SetDirty();

            // 文字色を変更する
            nuclearPowerUpkeepCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [合成石油工場維持コスト]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSyntheticOilSiteUpkeepCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(syntheticOilSiteUpkeepCostTextBox.Text, out val))
            {
                syntheticOilSiteUpkeepCostTextBox.Text = Misc.Economy.SyntheticOilSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                syntheticOilSiteUpkeepCostTextBox.Text = Misc.Economy.SyntheticOilSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SyntheticOilSiteUpkeepCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SyntheticOilSiteUpkeepCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SyntheticOilSiteUpkeepCost);
            Misc.SetDirty();

            // 文字色を変更する
            syntheticOilSiteUpkeepCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [合成希少資源工場維持コスト]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSyntheticRaresSiteUpkeepCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(syntheticRaresSiteUpkeepCostTextBox.Text, out val))
            {
                syntheticRaresSiteUpkeepCostTextBox.Text = Misc.Economy.SyntheticRaresSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                syntheticRaresSiteUpkeepCostTextBox.Text = Misc.Economy.SyntheticRaresSiteUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SyntheticRaresSiteUpkeepCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SyntheticRaresSiteUpkeepCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SyntheticRaresSiteUpkeepCost);
            Misc.SetDirty();

            // 文字色を変更する
            syntheticRaresSiteUpkeepCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍情報の存続期間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDurationDetectionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(durationDetectionTextBox.Text, out val))
            {
                durationDetectionTextBox.Text = Misc.Economy.DurationDetection.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                durationDetectionTextBox.Text = Misc.Economy.DurationDetection.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.DurationDetection)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DurationDetection = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DurationDetection);
            Misc.SetDirty();

            // 文字色を変更する
            durationDetectionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [船団攻撃回避時間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvoyProvinceHostileTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(convoyProvinceHostileTimeTextBox.Text, out val))
            {
                convoyProvinceHostileTimeTextBox.Text = Misc.Economy.ConvoyProvinceHostileTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                convoyProvinceHostileTimeTextBox.Text = Misc.Economy.ConvoyProvinceHostileTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.ConvoyProvinceHostileTime)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ConvoyProvinceHostileTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ConvoyProvinceHostileTime);
            Misc.SetDirty();

            // 文字色を変更する
            convoyProvinceHostileTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [船団攻撃妨害時間]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvoyProvinceBlockedTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(convoyProvinceBlockedTimeTextBox.Text, out val))
            {
                convoyProvinceBlockedTimeTextBox.Text = Misc.Economy.ConvoyProvinceBlockedTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                convoyProvinceBlockedTimeTextBox.Text = Misc.Economy.ConvoyProvinceBlockedTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.ConvoyProvinceBlockedTime)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ConvoyProvinceBlockedTime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ConvoyProvinceBlockedTime);
            Misc.SetDirty();

            // 文字色を変更する
            convoyProvinceBlockedTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [自動貿易に必要な輸送船団割合]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoTradeConvoyTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(autoTradeConvoyTextBox.Text, out val))
            {
                autoTradeConvoyTextBox.Text = Misc.Economy.AutoTradeConvoy.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                autoTradeConvoyTextBox.Text = Misc.Economy.AutoTradeConvoy.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.AutoTradeConvoy) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.AutoTradeConvoy = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.AutoTradeConvoy);
            Misc.SetDirty();

            // 文字色を変更する
            autoTradeConvoyTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報維持コスト]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyUpkeepCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spyUpkeepCostTextBox.Text, out val))
            {
                spyUpkeepCostTextBox.Text = Misc.Economy.SpyUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyUpkeepCostTextBox.Text = Misc.Economy.SpyUpkeepCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SpyUpkeepCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SpyUpkeepCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyUpkeepCost);
            Misc.SetDirty();

            // 文字色を変更する
            spyUpkeepCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [スパイ発見確率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyDetectionChanceTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spyDetectionChanceTextBox.Text, out val))
            {
                spyDetectionChanceTextBox.Text = Misc.Economy.SpyDetectionChance.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyDetectionChanceTextBox.Text = Misc.Economy.SpyDetectionChance.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SpyDetectionChance) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SpyDetectionChance = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyDetectionChance);
            Misc.SetDirty();

            // 文字色を変更する
            spyDetectionChanceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [インフラによるプロヴィンス効率補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInfraEfficiencyModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(infraEfficiencyModifierTextBox.Text, out val))
            {
                infraEfficiencyModifierTextBox.Text = Misc.Economy.InfraEfficiencyModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                infraEfficiencyModifierTextBox.Text = Misc.Economy.InfraEfficiencyModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.InfraEfficiencyModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.InfraEfficiencyModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.InfraEfficiencyModifier);
            Misc.SetDirty();

            // 文字色を変更する
            infraEfficiencyModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [人的資源の消費財生産補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerToConsumerGoodsTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerToConsumerGoodsTextBox.Text, out val))
            {
                manpowerToConsumerGoodsTextBox.Text = Misc.Economy.ManpowerToConsumerGoods.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerToConsumerGoodsTextBox.Text = Misc.Economy.ManpowerToConsumerGoods.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerToConsumerGoods) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerToConsumerGoods = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerToConsumerGoods);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerToConsumerGoodsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [スライダー移動の間隔]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeBetweenSliderChangesAoDTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(timeBetweenSliderChangesAoDTextBox.Text, out val))
            {
                timeBetweenSliderChangesAoDTextBox.Text = Misc.Economy.TimeBetweenSliderChangesAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                timeBetweenSliderChangesAoDTextBox.Text = Misc.Economy.TimeBetweenSliderChangesAoD.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.TimeBetweenSliderChangesAoD)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TimeBetweenSliderChangesAoD = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TimeBetweenSliderChangesAoD);
            Misc.SetDirty();

            // 文字色を変更する
            timeBetweenSliderChangesAoDTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海外プロヴィンスへの配置の必要IC]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinimalPlacementIcTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(minimalPlacementIcTextBox.Text, out val))
            {
                minimalPlacementIcTextBox.Text = Misc.Economy.MinimalPlacementIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                minimalPlacementIcTextBox.Text = Misc.Economy.MinimalPlacementIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MinimalPlacementIc) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MinimalPlacementIc = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinimalPlacementIc);
            Misc.SetDirty();

            // 文字色を変更する
            minimalPlacementIcTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [原子力発電量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNuclearPowerTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(nuclearPowerTextBox.Text, out val))
            {
                nuclearPowerTextBox.Text = Misc.Economy.NuclearPower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                nuclearPowerTextBox.Text = Misc.Economy.NuclearPower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.NuclearPower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.NuclearPower = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.NuclearPower);
            Misc.SetDirty();

            // 文字色を変更する
            nuclearPowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [インフラの自然回復率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFreeInfraRepairTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(freeInfraRepairTextBox.Text, out val))
            {
                freeInfraRepairTextBox.Text = Misc.Economy.FreeInfraRepair.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                freeInfraRepairTextBox.Text = Misc.Economy.FreeInfraRepair.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.FreeInfraRepair) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.FreeInfraRepair = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FreeInfraRepair);
            Misc.SetDirty();

            // 文字色を変更する
            freeInfraRepairTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [スライダー移動時の最大不満度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSliderDissentTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxSliderDissentTextBox.Text, out val))
            {
                maxSliderDissentTextBox.Text = Misc.Economy.MaxSliderDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxSliderDissentTextBox.Text = Misc.Economy.MaxSliderDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxSliderDissent) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxSliderDissent = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxSliderDissent);
            Misc.SetDirty();

            // 文字色を変更する
            maxSliderDissentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [スライダー移動時の最小不満度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinSliderDissentTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(minSliderDissentTextBox.Text, out val))
            {
                minSliderDissentTextBox.Text = Misc.Economy.MinSliderDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                minSliderDissentTextBox.Text = Misc.Economy.MinSliderDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MinSliderDissent) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MinSliderDissent = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinSliderDissent);
            Misc.SetDirty();

            // 文字色を変更する
            minSliderDissentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [スライダー移動可能な最大不満度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxDissentSliderMoveTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxDissentSliderMoveTextBox.Text, out val))
            {
                maxDissentSliderMoveTextBox.Text = Misc.Economy.MaxDissentSliderMove.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxDissentSliderMoveTextBox.Text = Misc.Economy.MaxDissentSliderMove.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxDissentSliderMove) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxDissentSliderMove = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxDissentSliderMove);
            Misc.SetDirty();

            // 文字色を変更する
            maxDissentSliderMoveTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [工場集中ボーナス]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcConcentrationBonusTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icConcentrationBonusTextBox.Text, out val))
            {
                icConcentrationBonusTextBox.Text = Misc.Economy.IcConcentrationBonus.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icConcentrationBonusTextBox.Text = Misc.Economy.IcConcentrationBonus.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcConcentrationBonus) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcConcentrationBonus = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcConcentrationBonus);
            Misc.SetDirty();

            // 文字色を変更する
            icConcentrationBonusTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [輸送艦変換係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransportConversionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(transportConversionTextBox.Text, out val))
            {
                transportConversionTextBox.Text = Misc.Economy.TransportConversion.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                transportConversionTextBox.Text = Misc.Economy.TransportConversion.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TransportConversion) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TransportConversion = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TransportConversion);
            Misc.SetDirty();

            // 文字色を変更する
            transportConversionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [閣僚変更遅延日数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterChangeDelayTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(ministerChangeDelayTextBox.Text, out val))
            {
                ministerChangeDelayTextBox.Text = Misc.Economy.MinisterChangeDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ministerChangeDelayTextBox.Text = Misc.Economy.MinisterChangeDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.MinisterChangeDelay)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MinisterChangeDelay = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinisterChangeDelay);
            Misc.SetDirty();

            // 文字色を変更する
            ministerChangeDelayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [閣僚変更遅延日数(イベント)]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterChangeEventDelayTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(ministerChangeEventDelayTextBox.Text, out val))
            {
                ministerChangeEventDelayTextBox.Text = Misc.Economy.MinisterChangeEventDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ministerChangeEventDelayTextBox.Text = Misc.Economy.MinisterChangeEventDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.MinisterChangeEventDelay)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MinisterChangeEventDelay = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinisterChangeEventDelay);
            Misc.SetDirty();

            // 文字色を変更する
            ministerChangeEventDelayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [国策変更遅延日数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdeaChangeDelayTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(ideaChangeDelayTextBox.Text, out val))
            {
                ideaChangeDelayTextBox.Text = Misc.Economy.IdeaChangeDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ideaChangeDelayTextBox.Text = Misc.Economy.IdeaChangeDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.IdeaChangeDelay)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IdeaChangeDelay = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IdeaChangeDelay);
            Misc.SetDirty();

            // 文字色を変更する
            ideaChangeDelayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [国策変更遅延日数(イベント)]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdeaChangeEventDelayTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(ideaChangeEventDelayTextBox.Text, out val))
            {
                ideaChangeEventDelayTextBox.Text = Misc.Economy.IdeaChangeEventDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ideaChangeEventDelayTextBox.Text = Misc.Economy.IdeaChangeEventDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.IdeaChangeEventDelay)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IdeaChangeEventDelay = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IdeaChangeEventDelay);
            Misc.SetDirty();

            // 文字色を変更する
            ideaChangeEventDelayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [指揮官変更遅延日数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeaderChangeDelayTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(leaderChangeDelayTextBox.Text, out val))
            {
                leaderChangeDelayTextBox.Text = Misc.Economy.LeaderChangeDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                leaderChangeDelayTextBox.Text = Misc.Economy.LeaderChangeDelay.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Economy.LeaderChangeDelay)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.LeaderChangeDelay = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.LeaderChangeDelay);
            Misc.SetDirty();

            // 文字色を変更する
            leaderChangeDelayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [国策変更時の不満度上昇量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeIdeaDissentTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(changeIdeaDissentTextBox.Text, out val))
            {
                changeIdeaDissentTextBox.Text = Misc.Economy.ChangeIdeaDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                changeIdeaDissentTextBox.Text = Misc.Economy.ChangeIdeaDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ChangeIdeaDissent) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ChangeIdeaDissent = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ChangeIdeaDissent);
            Misc.SetDirty();

            // 文字色を変更する
            changeIdeaDissentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [閣僚変更時の不満度上昇量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeMinisterDissentTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(changeMinisterDissentTextBox.Text, out val))
            {
                changeMinisterDissentTextBox.Text = Misc.Economy.ChangeMinisterDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                changeMinisterDissentTextBox.Text = Misc.Economy.ChangeMinisterDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ChangeMinisterDissent) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ChangeMinisterDissent = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ChangeMinisterDissent);
            Misc.SetDirty();

            // 文字色を変更する
            changeMinisterDissentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [反乱が発生する最低不満度]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinDissentRevoltTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(minDissentRevoltTextBox.Text, out val))
            {
                minDissentRevoltTextBox.Text = Misc.Economy.MinDissentRevolt.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                minDissentRevoltTextBox.Text = Misc.Economy.MinDissentRevolt.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MinDissentRevolt) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MinDissentRevolt = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinDissentRevolt);
            Misc.SetDirty();

            // 文字色を変更する
            minDissentRevoltTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [不満度による反乱軍発生率係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDissentRevoltMultiplierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(dissentRevoltMultiplierTextBox.Text, out val))
            {
                dissentRevoltMultiplierTextBox.Text = Misc.Economy.DissentRevoltMultiplier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                dissentRevoltMultiplierTextBox.Text = Misc.Economy.DissentRevoltMultiplier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DissentRevoltMultiplier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DissentRevoltMultiplier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DissentRevoltMultiplier);
            Misc.SetDirty();

            // 文字色を変更する
            dissentRevoltMultiplierTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 経済3タブ

        /// <summary>
        ///     経済3タブの項目を初期化する
        /// </summary>
        private void InitEconomy3Items()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                minAvailableIcLabel.Enabled = true;
                minFinalIcLabel.Enabled = true;
                dissentReductionLabel.Enabled = true;
                icMultiplierPuppetLabel.Enabled = true;
                resourceMultiplierNonNationalLabel.Enabled = true;
                resourceMultiplierNonOwnedLabel.Enabled = true;
                resourceMultiplierNonNationalAiLabel.Enabled = true;
                resourceMultiplierPuppetLabel.Enabled = true;
                manpowerMultiplierPuppetLabel.Enabled = true;
                manpowerMultiplierWartimeOverseaLabel.Enabled = true;
                manpowerMultiplierPeacetimeLabel.Enabled = true;
                manpowerMultiplierWartimeLabel.Enabled = true;
                dailyRetiredManpowerLabel.Enabled = true;
                reinforceToUpdateModifierLabel.Enabled = true;
                nationalismPerManpowerDhLabel.Enabled = true;
                maxNationalismLabel.Enabled = true;
                maxRevoltRiskLabel.Enabled = true;
                canUnitSendNonAlliedDhLabel.Enabled = true;
                bluePrintsCanSoldNonAlliedLabel.Enabled = true;
                provinceCanSoldNonAlliedLabel.Enabled = true;
                transferAlliedCoreProvincesLabel.Enabled = true;
                provinceBuildingsRepairModifierLabel.Enabled = true;
                provinceResourceRepairModifierLabel.Enabled = true;
                stockpileLimitMultiplierResourceLabel.Enabled = true;
                stockpileLimitMultiplierSuppliesOilLabel.Enabled = true;
                overStockpileLimitDailyLossLabel.Enabled = true;
                maxResourceDepotSizeLabel.Enabled = true;
                maxSuppliesOilDepotSizeLabel.Enabled = true;
                desiredStockPilesSuppliesOilLabel.Enabled = true;
                maxManpowerLabel.Enabled = true;
                convoyTransportsCapacityLabel.Enabled = true;
                suppyLandStaticDhLabel.Enabled = true;
                supplyLandBattleDhLabel.Enabled = true;
                fuelLandStaticLabel.Enabled = true;
                fuelLandBattleLabel.Enabled = true;
                supplyAirStaticDhLabel.Enabled = true;
                supplyAirBattleDhLabel.Enabled = true;
                fuelAirNavalStaticLabel.Enabled = true;
                fuelAirBattleLabel.Enabled = true;
                supplyNavalStaticDhLabel.Enabled = true;
                supplyNavalBattleDhLabel.Enabled = true;
                fuelNavalNotMovingLabel.Enabled = true;
                fuelNavalBattleLabel.Enabled = true;
                tpTransportsConversionRatioLabel.Enabled = true;
                ddEscortsConversionRatioLabel.Enabled = true;
                clEscortsConversionRatioLabel.Enabled = true;
                cvlEscortsConversionRatioLabel.Enabled = true;
                productionLineEditLabel.Enabled = true;
                gearingBonusLossUpgradeUnitLabel.Enabled = true;
                gearingBonusLossUpgradeBrigadeLabel.Enabled = true;
                dissentNukesLabel.Enabled = true;
                maxDailyDissentLabel.Enabled = true;

                minAvailableIcTextBox.Enabled = true;
                minFinalIcTextBox.Enabled = true;
                dissentReductionTextBox.Enabled = true;
                icMultiplierPuppetTextBox.Enabled = true;
                resourceMultiplierNonNationalTextBox.Enabled = true;
                resourceMultiplierNonOwnedTextBox.Enabled = true;
                resourceMultiplierNonNationalAiTextBox.Enabled = true;
                resourceMultiplierPuppetTextBox.Enabled = true;
                manpowerMultiplierPuppetTextBox.Enabled = true;
                manpowerMultiplierWartimeOverseaTextBox.Enabled = true;
                manpowerMultiplierPeacetimeTextBox.Enabled = true;
                manpowerMultiplierWartimeTextBox.Enabled = true;
                dailyRetiredManpowerTextBox.Enabled = true;
                reinforceToUpdateModifierTextBox.Enabled = true;
                nationalismPerManpowerDhTextBox.Enabled = true;
                maxNationalismTextBox.Enabled = true;
                maxRevoltRiskTextBox.Enabled = true;
                canUnitSendNonAlliedDhComboBox.Enabled = true;
                bluePrintsCanSoldNonAlliedComboBox.Enabled = true;
                provinceCanSoldNonAlliedComboBox.Enabled = true;
                transferAlliedCoreProvincesComboBox.Enabled = true;
                provinceBuildingsRepairModifierTextBox.Enabled = true;
                provinceResourceRepairModifierTextBox.Enabled = true;
                stockpileLimitMultiplierResourceTextBox.Enabled = true;
                stockpileLimitMultiplierSuppliesOilTextBox.Enabled = true;
                overStockpileLimitDailyLossTextBox.Enabled = true;
                maxResourceDepotSizeTextBox.Enabled = true;
                maxSuppliesOilDepotSizeTextBox.Enabled = true;
                desiredStockPilesSuppliesOilTextBox.Enabled = true;
                maxManpowerTextBox.Enabled = true;
                convoyTransportsCapacityTextBox.Enabled = true;
                suppyLandStaticDhTextBox.Enabled = true;
                supplyLandBattleDhTextBox.Enabled = true;
                fuelLandStaticTextBox.Enabled = true;
                fuelLandBattleTextBox.Enabled = true;
                supplyAirStaticDhTextBox.Enabled = true;
                supplyAirBattleDhTextBox.Enabled = true;
                fuelAirNavalStaticTextBox.Enabled = true;
                fuelAirBattleTextBox.Enabled = true;
                supplyNavalStaticDhTextBox.Enabled = true;
                supplyNavalBattleDhTextBox.Enabled = true;
                fuelNavalNotMovingTextBox.Enabled = true;
                fuelNavalBattleTextBox.Enabled = true;
                tpTransportsConversionRatioTextBox.Enabled = true;
                ddEscortsConversionRatioTextBox.Enabled = true;
                clEscortsConversionRatioTextBox.Enabled = true;
                cvlEscortsConversionRatioTextBox.Enabled = true;
                productionLineEditComboBox.Enabled = true;
                gearingBonusLossUpgradeUnitTextBox.Enabled = true;
                gearingBonusLossUpgradeBrigadeTextBox.Enabled = true;
                dissentNukesTextBox.Enabled = true;
                maxDailyDissentTextBox.Enabled = true;
            }
            else
            {
                minAvailableIcLabel.Enabled = false;
                minFinalIcLabel.Enabled = false;
                dissentReductionLabel.Enabled = false;
                icMultiplierPuppetLabel.Enabled = false;
                resourceMultiplierNonNationalLabel.Enabled = false;
                resourceMultiplierNonOwnedLabel.Enabled = false;
                resourceMultiplierNonNationalAiLabel.Enabled = false;
                resourceMultiplierPuppetLabel.Enabled = false;
                manpowerMultiplierPuppetLabel.Enabled = false;
                manpowerMultiplierWartimeOverseaLabel.Enabled = false;
                manpowerMultiplierPeacetimeLabel.Enabled = false;
                manpowerMultiplierWartimeLabel.Enabled = false;
                dailyRetiredManpowerLabel.Enabled = false;
                reinforceToUpdateModifierLabel.Enabled = false;
                nationalismPerManpowerDhLabel.Enabled = false;
                maxNationalismLabel.Enabled = false;
                maxRevoltRiskLabel.Enabled = false;
                canUnitSendNonAlliedDhLabel.Enabled = false;
                bluePrintsCanSoldNonAlliedLabel.Enabled = false;
                provinceCanSoldNonAlliedLabel.Enabled = false;
                transferAlliedCoreProvincesLabel.Enabled = false;
                provinceBuildingsRepairModifierLabel.Enabled = false;
                provinceResourceRepairModifierLabel.Enabled = false;
                stockpileLimitMultiplierResourceLabel.Enabled = false;
                stockpileLimitMultiplierSuppliesOilLabel.Enabled = false;
                overStockpileLimitDailyLossLabel.Enabled = false;
                maxResourceDepotSizeLabel.Enabled = false;
                maxSuppliesOilDepotSizeLabel.Enabled = false;
                desiredStockPilesSuppliesOilLabel.Enabled = false;
                maxManpowerLabel.Enabled = false;
                convoyTransportsCapacityLabel.Enabled = false;
                suppyLandStaticDhLabel.Enabled = false;
                supplyLandBattleDhLabel.Enabled = false;
                fuelLandStaticLabel.Enabled = false;
                fuelLandBattleLabel.Enabled = false;
                supplyAirStaticDhLabel.Enabled = false;
                supplyAirBattleDhLabel.Enabled = false;
                fuelAirNavalStaticLabel.Enabled = false;
                fuelAirBattleLabel.Enabled = false;
                supplyNavalStaticDhLabel.Enabled = false;
                supplyNavalBattleDhLabel.Enabled = false;
                fuelNavalNotMovingLabel.Enabled = false;
                fuelNavalBattleLabel.Enabled = false;
                tpTransportsConversionRatioLabel.Enabled = false;
                ddEscortsConversionRatioLabel.Enabled = false;
                clEscortsConversionRatioLabel.Enabled = false;
                cvlEscortsConversionRatioLabel.Enabled = false;
                productionLineEditLabel.Enabled = false;
                gearingBonusLossUpgradeUnitLabel.Enabled = false;
                gearingBonusLossUpgradeBrigadeLabel.Enabled = false;
                dissentNukesLabel.Enabled = false;
                maxDailyDissentLabel.Enabled = false;

                minAvailableIcTextBox.Enabled = false;
                minFinalIcTextBox.Enabled = false;
                dissentReductionTextBox.Enabled = false;
                icMultiplierPuppetTextBox.Enabled = false;
                resourceMultiplierNonNationalTextBox.Enabled = false;
                resourceMultiplierNonOwnedTextBox.Enabled = false;
                resourceMultiplierNonNationalAiTextBox.Enabled = false;
                resourceMultiplierPuppetTextBox.Enabled = false;
                manpowerMultiplierPuppetTextBox.Enabled = false;
                manpowerMultiplierWartimeOverseaTextBox.Enabled = false;
                manpowerMultiplierPeacetimeTextBox.Enabled = false;
                manpowerMultiplierWartimeTextBox.Enabled = false;
                dailyRetiredManpowerTextBox.Enabled = false;
                reinforceToUpdateModifierTextBox.Enabled = false;
                nationalismPerManpowerDhTextBox.Enabled = false;
                maxNationalismTextBox.Enabled = false;
                maxRevoltRiskTextBox.Enabled = false;
                canUnitSendNonAlliedDhComboBox.Enabled = false;
                bluePrintsCanSoldNonAlliedComboBox.Enabled = false;
                provinceCanSoldNonAlliedComboBox.Enabled = false;
                transferAlliedCoreProvincesComboBox.Enabled = false;
                provinceBuildingsRepairModifierTextBox.Enabled = false;
                provinceResourceRepairModifierTextBox.Enabled = false;
                stockpileLimitMultiplierResourceTextBox.Enabled = false;
                stockpileLimitMultiplierSuppliesOilTextBox.Enabled = false;
                overStockpileLimitDailyLossTextBox.Enabled = false;
                maxResourceDepotSizeTextBox.Enabled = false;
                maxSuppliesOilDepotSizeTextBox.Enabled = false;
                desiredStockPilesSuppliesOilTextBox.Enabled = false;
                maxManpowerTextBox.Enabled = false;
                convoyTransportsCapacityTextBox.Enabled = false;
                suppyLandStaticDhTextBox.Enabled = false;
                supplyLandBattleDhTextBox.Enabled = false;
                fuelLandStaticTextBox.Enabled = false;
                fuelLandBattleTextBox.Enabled = false;
                supplyAirStaticDhTextBox.Enabled = false;
                supplyAirBattleDhTextBox.Enabled = false;
                fuelAirNavalStaticTextBox.Enabled = false;
                fuelAirBattleTextBox.Enabled = false;
                supplyNavalStaticDhTextBox.Enabled = false;
                supplyNavalBattleDhTextBox.Enabled = false;
                fuelNavalNotMovingTextBox.Enabled = false;
                fuelNavalBattleTextBox.Enabled = false;
                tpTransportsConversionRatioTextBox.Enabled = false;
                ddEscortsConversionRatioTextBox.Enabled = false;
                clEscortsConversionRatioTextBox.Enabled = false;
                cvlEscortsConversionRatioTextBox.Enabled = false;
                productionLineEditComboBox.Enabled = false;
                gearingBonusLossUpgradeUnitTextBox.Enabled = false;
                gearingBonusLossUpgradeBrigadeTextBox.Enabled = false;
                dissentNukesTextBox.Enabled = false;
                maxDailyDissentTextBox.Enabled = false;

                canUnitSendNonAlliedDhComboBox.SelectedIndex = -1;
                canUnitSendNonAlliedDhComboBox.ResetText();
                bluePrintsCanSoldNonAlliedComboBox.SelectedIndex = -1;
                bluePrintsCanSoldNonAlliedComboBox.ResetText();
                provinceCanSoldNonAlliedComboBox.SelectedIndex = -1;
                provinceCanSoldNonAlliedComboBox.ResetText();
                transferAlliedCoreProvincesComboBox.SelectedIndex = -1;
                transferAlliedCoreProvincesComboBox.ResetText();
                productionLineEditComboBox.SelectedIndex = -1;
                productionLineEditComboBox.ResetText();
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                nukesProductionModifierLabel.Enabled = true;
                convoySystemOptionsAlliedLabel.Enabled = true;
                resourceConvoysBackUnneededLabel.Enabled = true;
            
                nukesProductionModifierTextBox.Enabled = true;
                convoySystemOptionsAlliedComboBox.Enabled = true;
                resourceConvoysBackUnneededTextBox.Enabled = true;
            }
            else
            {
                nukesProductionModifierLabel.Enabled = false;
                convoySystemOptionsAlliedLabel.Enabled = false;
                resourceConvoysBackUnneededLabel.Enabled = false;

                nukesProductionModifierTextBox.Enabled = false;
                convoySystemOptionsAlliedComboBox.Enabled = false;
                resourceConvoysBackUnneededTextBox.Enabled = false;

                convoySystemOptionsAlliedComboBox.SelectedIndex = -1;
                convoySystemOptionsAlliedComboBox.ResetText();
            }
        }

        /// <summary>
        ///     経済3タブの項目を更新する
        /// </summary>
        private void UpdateEconomy3Items()
        {
            // 編集項目の値を更新する
            if (Game.Type == GameType.DarkestHour)
            {
                minAvailableIcTextBox.Text = Misc.Economy.MinAvailableIc.ToString(CultureInfo.InvariantCulture);
                minFinalIcTextBox.Text = Misc.Economy.MinFinalIc.ToString(CultureInfo.InvariantCulture);
                dissentReductionTextBox.Text = Misc.Economy.DissentReduction.ToString(CultureInfo.InvariantCulture);
                icMultiplierPuppetTextBox.Text = Misc.Economy.IcMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                resourceMultiplierNonNationalTextBox.Text = Misc.Economy.ResourceMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
                resourceMultiplierNonOwnedTextBox.Text = Misc.Economy.ResourceMultiplierNonOwned.ToString(CultureInfo.InvariantCulture);
                resourceMultiplierNonNationalAiTextBox.Text = Misc.Economy.ResourceMultiplierNonNationalAi.ToString(CultureInfo.InvariantCulture);
                resourceMultiplierPuppetTextBox.Text = Misc.Economy.ResourceMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                manpowerMultiplierPuppetTextBox.Text = Misc.Economy.ManpowerMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                manpowerMultiplierWartimeOverseaTextBox.Text = Misc.Economy.ManpowerMultiplierWartimeOversea.ToString(CultureInfo.InvariantCulture);
                manpowerMultiplierPeacetimeTextBox.Text = Misc.Economy.ManpowerMultiplierPeacetime.ToString(CultureInfo.InvariantCulture);
                manpowerMultiplierWartimeTextBox.Text = Misc.Economy.ManpowerMultiplierWartime.ToString(CultureInfo.InvariantCulture);
                dailyRetiredManpowerTextBox.Text = Misc.Economy.DailyRetiredManpower.ToString(CultureInfo.InvariantCulture);
                reinforceToUpdateModifierTextBox.Text = Misc.Economy.ReinforceToUpdateModifier.ToString(CultureInfo.InvariantCulture);
                nationalismPerManpowerDhTextBox.Text = Misc.Economy.NationalismPerManpowerDh.ToString(CultureInfo.InvariantCulture);
                maxNationalismTextBox.Text = Misc.Economy.MaxNationalism.ToString(CultureInfo.InvariantCulture);
                maxRevoltRiskTextBox.Text = Misc.Economy.MaxRevoltRisk.ToString(CultureInfo.InvariantCulture);
                canUnitSendNonAlliedDhComboBox.SelectedIndex = Misc.Economy.CanUnitSendNonAlliedDh;
                bluePrintsCanSoldNonAlliedComboBox.SelectedIndex = Misc.Economy.BluePrintsCanSoldNonAllied;
                provinceCanSoldNonAlliedComboBox.SelectedIndex = Misc.Economy.ProvinceCanSoldNonAllied;
                transferAlliedCoreProvincesComboBox.SelectedIndex = Misc.Economy.TransferAlliedCoreProvinces ? 1 : 0;
                provinceBuildingsRepairModifierTextBox.Text = Misc.Economy.ProvinceBuildingsRepairModifier.ToString(CultureInfo.InvariantCulture);
                provinceResourceRepairModifierTextBox.Text = Misc.Economy.ProvinceResourceRepairModifier.ToString(CultureInfo.InvariantCulture);
                stockpileLimitMultiplierResourceTextBox.Text = Misc.Economy.StockpileLimitMultiplierResource.ToString(CultureInfo.InvariantCulture);
                stockpileLimitMultiplierSuppliesOilTextBox.Text = Misc.Economy.StockpileLimitMultiplierSuppliesOil.ToString(CultureInfo.InvariantCulture);
                overStockpileLimitDailyLossTextBox.Text = Misc.Economy.OverStockpileLimitDailyLoss.ToString(CultureInfo.InvariantCulture);
                maxResourceDepotSizeTextBox.Text = Misc.Economy.MaxResourceDepotSize.ToString(CultureInfo.InvariantCulture);
                maxSuppliesOilDepotSizeTextBox.Text = Misc.Economy.MaxSuppliesOilDepotSize.ToString(CultureInfo.InvariantCulture);
                desiredStockPilesSuppliesOilTextBox.Text = Misc.Economy.DesiredStockPilesSuppliesOil.ToString(CultureInfo.InvariantCulture);
                maxManpowerTextBox.Text = Misc.Economy.MaxManpower.ToString(CultureInfo.InvariantCulture);
                convoyTransportsCapacityTextBox.Text = Misc.Economy.ConvoyTransportsCapacity.ToString(CultureInfo.InvariantCulture);
                suppyLandStaticDhTextBox.Text = Misc.Economy.SuppyLandStaticDh.ToString(CultureInfo.InvariantCulture);
                supplyLandBattleDhTextBox.Text = Misc.Economy.SupplyLandBattleDh.ToString(CultureInfo.InvariantCulture);
                fuelLandStaticTextBox.Text = Misc.Economy.FuelLandStatic.ToString(CultureInfo.InvariantCulture);
                fuelLandBattleTextBox.Text = Misc.Economy.FuelLandBattle.ToString(CultureInfo.InvariantCulture);
                supplyAirStaticDhTextBox.Text = Misc.Economy.SupplyAirStaticDh.ToString(CultureInfo.InvariantCulture);
                supplyAirBattleDhTextBox.Text = Misc.Economy.SupplyAirBattleDh.ToString(CultureInfo.InvariantCulture);
                fuelAirNavalStaticTextBox.Text = Misc.Economy.FuelAirNavalStatic.ToString(CultureInfo.InvariantCulture);
                fuelAirBattleTextBox.Text = Misc.Economy.FuelAirBattle.ToString(CultureInfo.InvariantCulture);
                supplyNavalStaticDhTextBox.Text = Misc.Economy.SupplyNavalStaticDh.ToString(CultureInfo.InvariantCulture);
                supplyNavalBattleDhTextBox.Text = Misc.Economy.SupplyNavalBattleDh.ToString(CultureInfo.InvariantCulture);
                fuelNavalNotMovingTextBox.Text = Misc.Economy.FuelNavalNotMoving.ToString(CultureInfo.InvariantCulture);
                fuelNavalBattleTextBox.Text = Misc.Economy.FuelNavalBattle.ToString(CultureInfo.InvariantCulture);
                tpTransportsConversionRatioTextBox.Text = Misc.Economy.TpTransportsConversionRatio.ToString(CultureInfo.InvariantCulture);
                ddEscortsConversionRatioTextBox.Text = Misc.Economy.DdEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                clEscortsConversionRatioTextBox.Text = Misc.Economy.ClEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                cvlEscortsConversionRatioTextBox.Text = Misc.Economy.CvlEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                productionLineEditComboBox.SelectedIndex = Misc.Economy.ProductionLineEdit ? 1 : 0;
                gearingBonusLossUpgradeUnitTextBox.Text = Misc.Economy.GearingBonusLossUpgradeUnit.ToString(CultureInfo.InvariantCulture);
                gearingBonusLossUpgradeBrigadeTextBox.Text = Misc.Economy.GearingBonusLossUpgradeBrigade.ToString(CultureInfo.InvariantCulture);
                dissentNukesTextBox.Text = Misc.Economy.DissentNukes.ToString(CultureInfo.InvariantCulture);
                maxDailyDissentTextBox.Text = Misc.Economy.MaxDailyDissent.ToString(CultureInfo.InvariantCulture);
            }

            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                nukesProductionModifierTextBox.Text = Misc.Economy.NukesProductionModifier.ToString(CultureInfo.InvariantCulture);
                convoySystemOptionsAlliedComboBox.SelectedIndex = Misc.Economy.ConvoySystemOptionsAllied;
                resourceConvoysBackUnneededTextBox.Text = Misc.Economy.ResourceConvoysBackUnneeded.ToString(CultureInfo.InvariantCulture);
            }

            // 編集項目の色を更新する
            if (Game.Type == GameType.DarkestHour)
            {
                minAvailableIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinAvailableIc) ? Color.Red : SystemColors.WindowText;
                minFinalIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinFinalIc) ? Color.Red : SystemColors.WindowText;
                dissentReductionTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentReduction) ? Color.Red : SystemColors.WindowText;
                icMultiplierPuppetTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcMultiplierPuppet) ? Color.Red : SystemColors.WindowText;
                resourceMultiplierNonNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceMultiplierNonNational) ? Color.Red : SystemColors.WindowText;
                resourceMultiplierNonOwnedTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceMultiplierNonOwned) ? Color.Red : SystemColors.WindowText;
                resourceMultiplierNonNationalAiTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceMultiplierNonNationalAi) ? Color.Red : SystemColors.WindowText;
                resourceMultiplierPuppetTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceMultiplierPuppet) ? Color.Red : SystemColors.WindowText;
                manpowerMultiplierPuppetTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierPuppet) ? Color.Red : SystemColors.WindowText;
                manpowerMultiplierWartimeOverseaTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierWartimeOversea) ? Color.Red : SystemColors.WindowText;
                manpowerMultiplierPeacetimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierPeacetime) ? Color.Red : SystemColors.WindowText;
                manpowerMultiplierWartimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierWartime) ? Color.Red : SystemColors.WindowText;
                dailyRetiredManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.DailyRetiredManpower) ? Color.Red : SystemColors.WindowText;
                reinforceToUpdateModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceToUpdateModifier) ? Color.Red : SystemColors.WindowText;
                nationalismPerManpowerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.NationalismPerManpowerDh) ? Color.Red : SystemColors.WindowText;
                maxNationalismTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxNationalism) ? Color.Red : SystemColors.WindowText;
                maxRevoltRiskTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxRevoltRisk) ? Color.Red : SystemColors.WindowText;
                provinceBuildingsRepairModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ProvinceBuildingsRepairModifier) ? Color.Red : SystemColors.WindowText;
                provinceResourceRepairModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ProvinceResourceRepairModifier) ? Color.Red : SystemColors.WindowText;
                stockpileLimitMultiplierResourceTextBox.ForeColor = Misc.IsDirty(MiscItemId.StockpileLimitMultiplierResource) ? Color.Red : SystemColors.WindowText;
                stockpileLimitMultiplierSuppliesOilTextBox.ForeColor = Misc.IsDirty(MiscItemId.StockpileLimitMultiplierSuppliesOil) ? Color.Red : SystemColors.WindowText;
                overStockpileLimitDailyLossTextBox.ForeColor = Misc.IsDirty(MiscItemId.OverStockpileLimitDailyLoss) ? Color.Red : SystemColors.WindowText;
                maxResourceDepotSizeTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxResourceDepotSize) ? Color.Red : SystemColors.WindowText;
                maxSuppliesOilDepotSizeTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxSuppliesOilDepotSize) ? Color.Red : SystemColors.WindowText;
                desiredStockPilesSuppliesOilTextBox.ForeColor = Misc.IsDirty(MiscItemId.DesiredStockPilesSuppliesOil) ? Color.Red : SystemColors.WindowText;
                maxManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxManpower) ? Color.Red : SystemColors.WindowText;
                convoyTransportsCapacityTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyTransportsCapacity) ? Color.Red : SystemColors.WindowText;
                suppyLandStaticDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SuppyLandStaticDh) ? Color.Red : SystemColors.WindowText;
                supplyLandBattleDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandBattleDh) ? Color.Red : SystemColors.WindowText;
                fuelLandStaticTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelLandStatic) ? Color.Red : SystemColors.WindowText;
                fuelLandBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelLandBattle) ? Color.Red : SystemColors.WindowText;
                supplyAirStaticDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirStaticDh) ? Color.Red : SystemColors.WindowText;
                supplyAirBattleDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirBattleDh) ? Color.Red : SystemColors.WindowText;
                fuelAirNavalStaticTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelAirNavalStatic) ? Color.Red : SystemColors.WindowText;
                fuelAirBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelAirBattle) ? Color.Red : SystemColors.WindowText;
                supplyNavalStaticDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalStaticDh) ? Color.Red : SystemColors.WindowText;
                supplyNavalBattleDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalBattleDh) ? Color.Red : SystemColors.WindowText;
                fuelNavalNotMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelNavalNotMoving) ? Color.Red : SystemColors.WindowText;
                fuelNavalBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelNavalBattle) ? Color.Red : SystemColors.WindowText;
                tpTransportsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.TpTransportsConversionRatio) ? Color.Red : SystemColors.WindowText;
                ddEscortsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.DdEscortsConversionRatio) ? Color.Red : SystemColors.WindowText;
                clEscortsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.ClEscortsConversionRatio) ? Color.Red : SystemColors.WindowText;
                cvlEscortsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.CvlEscortsConversionRatio) ? Color.Red : SystemColors.WindowText;
                gearingBonusLossUpgradeUnitTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingBonusLossUpgradeUnit) ? Color.Red : SystemColors.WindowText;
                gearingBonusLossUpgradeBrigadeTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingBonusLossUpgradeBrigade) ? Color.Red : SystemColors.WindowText;
                dissentNukesTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentNukes) ? Color.Red : SystemColors.WindowText;
                maxDailyDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxDailyDissent) ? Color.Red : SystemColors.WindowText;
            }

            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                nukesProductionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.NukesProductionModifier) ? Color.Red : SystemColors.WindowText;
                resourceConvoysBackUnneededTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceConvoysBackUnneeded) ? Color.Red : SystemColors.WindowText;
            }
        }

        /// <summary>
        /// [最小実効ICの比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinAvailableIcTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(minAvailableIcTextBox.Text, out val))
            {
                minAvailableIcTextBox.Text = Misc.Economy.MinAvailableIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                minAvailableIcTextBox.Text = Misc.Economy.MinAvailableIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MinAvailableIc) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MinAvailableIc = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinAvailableIc);
            Misc.SetDirty();

            // 文字色を変更する
            minAvailableIcTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [最小実効IC]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinFinalIcTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(minFinalIcTextBox.Text, out val))
            {
                minFinalIcTextBox.Text = Misc.Economy.MinFinalIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                minFinalIcTextBox.Text = Misc.Economy.MinFinalIc.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MinFinalIc) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MinFinalIc = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinFinalIc);
            Misc.SetDirty();

            // 文字色を変更する
            minFinalIcTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [不満度低下補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDissentReductionTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(dissentReductionTextBox.Text, out val))
            {
                dissentReductionTextBox.Text = Misc.Economy.DissentReduction.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                dissentReductionTextBox.Text = Misc.Economy.DissentReduction.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DissentReduction) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DissentReduction = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DissentReduction);
            Misc.SetDirty();

            // 文字色を変更する
            dissentReductionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [属国のIC補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcMultiplierPuppetTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icMultiplierPuppetTextBox.Text, out val))
            {
                icMultiplierPuppetTextBox.Text = Misc.Economy.IcMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icMultiplierPuppetTextBox.Text = Misc.Economy.IcMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.IcMultiplierPuppet) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.IcMultiplierPuppet = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcMultiplierPuppet);
            Misc.SetDirty();

            // 文字色を変更する
            icMultiplierPuppetTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [非中核州の資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceMultiplierNonNationalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(resourceMultiplierNonNationalTextBox.Text, out val))
            {
                resourceMultiplierNonNationalTextBox.Text = Misc.Economy.ResourceMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                resourceMultiplierNonNationalTextBox.Text = Misc.Economy.ResourceMultiplierNonNational.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ResourceMultiplierNonNational) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ResourceMultiplierNonNational = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ResourceMultiplierNonNational);
            Misc.SetDirty();

            // 文字色を変更する
            resourceMultiplierNonNationalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [占領地の資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceMultiplierNonOwnedTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(resourceMultiplierNonOwnedTextBox.Text, out val))
            {
                resourceMultiplierNonOwnedTextBox.Text = Misc.Economy.ResourceMultiplierNonOwned.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                resourceMultiplierNonOwnedTextBox.Text = Misc.Economy.ResourceMultiplierNonOwned.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ResourceMultiplierNonOwned) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ResourceMultiplierNonOwned = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ResourceMultiplierNonOwned);
            Misc.SetDirty();

            // 文字色を変更する
            resourceMultiplierNonOwnedTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [非中核州の資源補正(AI)]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceMultiplierNonNationalAiTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(resourceMultiplierNonNationalAiTextBox.Text, out val))
            {
                resourceMultiplierNonNationalAiTextBox.Text = Misc.Economy.ResourceMultiplierNonNationalAi.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                resourceMultiplierNonNationalAiTextBox.Text = Misc.Economy.ResourceMultiplierNonNationalAi.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ResourceMultiplierNonNationalAi) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ResourceMultiplierNonNationalAi = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ResourceMultiplierNonNationalAi);
            Misc.SetDirty();

            // 文字色を変更する
            resourceMultiplierNonNationalAiTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [属国の資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceMultiplierPuppetTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(resourceMultiplierPuppetTextBox.Text, out val))
            {
                resourceMultiplierPuppetTextBox.Text = Misc.Economy.ResourceMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                resourceMultiplierPuppetTextBox.Text = Misc.Economy.ResourceMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ResourceMultiplierPuppet) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ResourceMultiplierPuppet = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ResourceMultiplierPuppet);
            Misc.SetDirty();

            // 文字色を変更する
            resourceMultiplierPuppetTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [属国の人的資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerMultiplierPuppetTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerMultiplierPuppetTextBox.Text, out val))
            {
                manpowerMultiplierPuppetTextBox.Text = Misc.Economy.ManpowerMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerMultiplierPuppetTextBox.Text = Misc.Economy.ManpowerMultiplierPuppet.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerMultiplierPuppet) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerMultiplierPuppet = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerMultiplierPuppet);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerMultiplierPuppetTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [戦時の海外州の人的資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerMultiplierWartimeOverseaTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerMultiplierWartimeOverseaTextBox.Text, out val))
            {
                manpowerMultiplierWartimeOverseaTextBox.Text = Misc.Economy.ManpowerMultiplierWartimeOversea.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerMultiplierWartimeOverseaTextBox.Text = Misc.Economy.ManpowerMultiplierWartimeOversea.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerMultiplierWartimeOversea) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerMultiplierWartimeOversea = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerMultiplierWartimeOversea);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerMultiplierWartimeOverseaTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [平時の人的資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerMultiplierPeacetimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerMultiplierPeacetimeTextBox.Text, out val))
            {
                manpowerMultiplierPeacetimeTextBox.Text = Misc.Economy.ManpowerMultiplierPeacetime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerMultiplierPeacetimeTextBox.Text = Misc.Economy.ManpowerMultiplierPeacetime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerMultiplierPeacetime) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerMultiplierPeacetime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerMultiplierPeacetime);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerMultiplierPeacetimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [戦時の人的資源補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManpowerMultiplierWartimeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manpowerMultiplierWartimeTextBox.Text, out val))
            {
                manpowerMultiplierWartimeTextBox.Text = Misc.Economy.ManpowerMultiplierWartime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                manpowerMultiplierWartimeTextBox.Text = Misc.Economy.ManpowerMultiplierWartime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ManpowerMultiplierWartime) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ManpowerMultiplierWartime = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ManpowerMultiplierWartime);
            Misc.SetDirty();

            // 文字色を変更する
            manpowerMultiplierWartimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [人的資源の老化率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDailyRetiredManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(dailyRetiredManpowerTextBox.Text, out val))
            {
                dailyRetiredManpowerTextBox.Text = Misc.Economy.DailyRetiredManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                dailyRetiredManpowerTextBox.Text = Misc.Economy.DailyRetiredManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DailyRetiredManpower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DailyRetiredManpower = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DailyRetiredManpower);
            Misc.SetDirty();

            // 文字色を変更する
            dailyRetiredManpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [改良のための補充係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceToUpdateModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(reinforceToUpdateModifierTextBox.Text, out val))
            {
                reinforceToUpdateModifierTextBox.Text = Misc.Economy.ReinforceToUpdateModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                reinforceToUpdateModifierTextBox.Text = Misc.Economy.ReinforceToUpdateModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ReinforceToUpdateModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ReinforceToUpdateModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ReinforceToUpdateModifier);
            Misc.SetDirty();

            // 文字色を変更する
            reinforceToUpdateModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [人的資源によるナショナリズムの補正値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNationalismPerManpowerDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(nationalismPerManpowerDhTextBox.Text, out val))
            {
                nationalismPerManpowerDhTextBox.Text = Misc.Economy.NationalismPerManpowerDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                nationalismPerManpowerDhTextBox.Text = Misc.Economy.NationalismPerManpowerDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.NationalismPerManpowerDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.NationalismPerManpowerDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.NationalismPerManpowerDh);
            Misc.SetDirty();

            // 文字色を変更する
            nationalismPerManpowerDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [ナショナリズム最大値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxNationalismTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxNationalismTextBox.Text, out val))
            {
                maxNationalismTextBox.Text = Misc.Economy.MaxNationalism.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxNationalismTextBox.Text = Misc.Economy.MaxNationalism.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxNationalism) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxNationalism = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxNationalism);
            Misc.SetDirty();

            // 文字色を変更する
            maxNationalismTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [最大反乱率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxRevoltRiskTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxRevoltRiskTextBox.Text, out val))
            {
                maxRevoltRiskTextBox.Text = Misc.Economy.MaxRevoltRisk.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxRevoltRiskTextBox.Text = Misc.Economy.MaxRevoltRisk.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxRevoltRisk) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxRevoltRisk = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxRevoltRisk);
            Misc.SetDirty();

            // 文字色を変更する
            maxRevoltRiskTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [非同盟国に師団を譲渡できるかどうか]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanUnitSendNonAlliedDhComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.CanUnitSendNonAlliedDh;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.CanUnitSendNonAlliedDh))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = canUnitSendNonAlliedDhComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [非同盟国に師団を譲渡できるかどうか]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanUnitSendNonAlliedDhComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (canUnitSendNonAlliedDhComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = canUnitSendNonAlliedDhComboBox.SelectedIndex;
            if (val == Misc.Economy.CanUnitSendNonAlliedDh)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CanUnitSendNonAlliedDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CanUnitSendNonAlliedDh);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            canUnitSendNonAlliedDhComboBox.Refresh();
        }

        /// <summary>
        ///     [非同盟国に青写真の売却を許可]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBluePrintsCanSoldNonAlliedComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.BluePrintsCanSoldNonAllied;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.BluePrintsCanSoldNonAllied))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = bluePrintsCanSoldNonAlliedComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [非同盟国に青写真の売却を許可]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBluePrintsCanSoldNonAlliedComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (bluePrintsCanSoldNonAlliedComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = bluePrintsCanSoldNonAlliedComboBox.SelectedIndex;
            if (val == Misc.Economy.BluePrintsCanSoldNonAllied)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.BluePrintsCanSoldNonAllied = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.BluePrintsCanSoldNonAllied);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            bluePrintsCanSoldNonAlliedComboBox.Refresh();
        }

        /// <summary>
        ///     [非同盟国にプロヴィンスの売却/譲渡を許可]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceCanSoldNonAlliedComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.ProvinceCanSoldNonAllied;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.ProvinceCanSoldNonAllied))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = provinceCanSoldNonAlliedComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [非同盟国にプロヴィンスの売却/譲渡を許可]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceCanSoldNonAllieddComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (provinceCanSoldNonAlliedComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = provinceCanSoldNonAlliedComboBox.SelectedIndex;
            if (val == Misc.Economy.ProvinceCanSoldNonAllied)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ProvinceCanSoldNonAllied = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ProvinceCanSoldNonAllied);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            provinceCanSoldNonAlliedComboBox.Refresh();
        }

        /// <summary>
        ///     [占領中の同盟国の中核州返還を許可]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransferAlliedCoreProvincesComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.TransferAlliedCoreProvinces ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.TransferAlliedCoreProvinces))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = transferAlliedCoreProvincesComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [占領中の同盟国の中核州返還を許可]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransferAlliedCoreProvincesComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (transferAlliedCoreProvincesComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (transferAlliedCoreProvincesComboBox.SelectedIndex == 1);
            if (val == Misc.Economy.TransferAlliedCoreProvinces)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TransferAlliedCoreProvinces = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TransferAlliedCoreProvinces);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            transferAlliedCoreProvincesComboBox.Refresh();
        }

        /// <summary>
        /// [建物修復速度補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceBuildingsRepairModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(provinceBuildingsRepairModifierTextBox.Text, out val))
            {
                provinceBuildingsRepairModifierTextBox.Text = Misc.Economy.ProvinceBuildingsRepairModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                provinceBuildingsRepairModifierTextBox.Text = Misc.Economy.ProvinceBuildingsRepairModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ProvinceBuildingsRepairModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ProvinceBuildingsRepairModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ProvinceBuildingsRepairModifier);
            Misc.SetDirty();

            // 文字色を変更する
            provinceBuildingsRepairModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [資源回復速度補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceResourceRepairModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(provinceResourceRepairModifierTextBox.Text, out val))
            {
                provinceResourceRepairModifierTextBox.Text = Misc.Economy.ProvinceResourceRepairModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                provinceResourceRepairModifierTextBox.Text = Misc.Economy.ProvinceResourceRepairModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ProvinceResourceRepairModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ProvinceResourceRepairModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ProvinceResourceRepairModifier);
            Misc.SetDirty();

            // 文字色を変更する
            provinceResourceRepairModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [資源備蓄上限補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStockpileLimitMultiplierResourceTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(stockpileLimitMultiplierResourceTextBox.Text, out val))
            {
                stockpileLimitMultiplierResourceTextBox.Text = Misc.Economy.StockpileLimitMultiplierResource.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                stockpileLimitMultiplierResourceTextBox.Text = Misc.Economy.StockpileLimitMultiplierResource.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.StockpileLimitMultiplierResource) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.StockpileLimitMultiplierResource = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.StockpileLimitMultiplierResource);
            Misc.SetDirty();

            // 文字色を変更する
            stockpileLimitMultiplierResourceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [物資/燃料備蓄上限補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStockpileLimitMultiplierSuppliesOilTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(stockpileLimitMultiplierSuppliesOilTextBox.Text, out val))
            {
                stockpileLimitMultiplierSuppliesOilTextBox.Text = Misc.Economy.StockpileLimitMultiplierSuppliesOil.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                stockpileLimitMultiplierSuppliesOilTextBox.Text = Misc.Economy.StockpileLimitMultiplierSuppliesOil.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.StockpileLimitMultiplierSuppliesOil) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.StockpileLimitMultiplierSuppliesOil = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.StockpileLimitMultiplierSuppliesOil);
            Misc.SetDirty();

            // 文字色を変更する
            stockpileLimitMultiplierSuppliesOilTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [超過備蓄損失割合]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOverStockpileLimitDailyLossTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(overStockpileLimitDailyLossTextBox.Text, out val))
            {
                overStockpileLimitDailyLossTextBox.Text = Misc.Economy.OverStockpileLimitDailyLoss.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                overStockpileLimitDailyLossTextBox.Text = Misc.Economy.OverStockpileLimitDailyLoss.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.OverStockpileLimitDailyLoss) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.OverStockpileLimitDailyLoss = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.OverStockpileLimitDailyLoss);
            Misc.SetDirty();

            // 文字色を変更する
            overStockpileLimitDailyLossTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [資源備蓄上限値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxResourceDepotSizeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxResourceDepotSizeTextBox.Text, out val))
            {
                maxResourceDepotSizeTextBox.Text = Misc.Economy.MaxResourceDepotSize.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxResourceDepotSizeTextBox.Text = Misc.Economy.MaxResourceDepotSize.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxResourceDepotSize) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxResourceDepotSize = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxResourceDepotSize);
            Misc.SetDirty();

            // 文字色を変更する
            maxResourceDepotSizeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [物資/燃料備蓄上限値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSuppliesOilDepotSizeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxSuppliesOilDepotSizeTextBox.Text, out val))
            {
                maxSuppliesOilDepotSizeTextBox.Text = Misc.Economy.MaxSuppliesOilDepotSize.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxSuppliesOilDepotSizeTextBox.Text = Misc.Economy.MaxSuppliesOilDepotSize.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxSuppliesOilDepotSize) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxSuppliesOilDepotSize = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxSuppliesOilDepotSize);
            Misc.SetDirty();

            // 文字色を変更する
            maxSuppliesOilDepotSizeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [理想物資/燃料備蓄比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDesiredStockPilesSuppliesOilTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(desiredStockPilesSuppliesOilTextBox.Text, out val))
            {
                desiredStockPilesSuppliesOilTextBox.Text = Misc.Economy.DesiredStockPilesSuppliesOil.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                desiredStockPilesSuppliesOilTextBox.Text = Misc.Economy.DesiredStockPilesSuppliesOil.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DesiredStockPilesSuppliesOil) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DesiredStockPilesSuppliesOil = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DesiredStockPilesSuppliesOil);
            Misc.SetDirty();

            // 文字色を変更する
            desiredStockPilesSuppliesOilTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [最大人的資源]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxManpowerTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxManpowerTextBox.Text, out val))
            {
                maxManpowerTextBox.Text = Misc.Economy.MaxManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxManpowerTextBox.Text = Misc.Economy.MaxManpower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxManpower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxManpower = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxManpower);
            Misc.SetDirty();

            // 文字色を変更する
            maxManpowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [船団輸送能力]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvoyTransportsCapacityTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(convoyTransportsCapacityTextBox.Text, out val))
            {
                convoyTransportsCapacityTextBox.Text = Misc.Economy.ConvoyTransportsCapacity.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                convoyTransportsCapacityTextBox.Text = Misc.Economy.ConvoyTransportsCapacity.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ConvoyTransportsCapacity) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ConvoyTransportsCapacity = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ConvoyTransportsCapacity);
            Misc.SetDirty();

            // 文字色を変更する
            convoyTransportsCapacityTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の待機時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSuppyLandStaticDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(suppyLandStaticDhTextBox.Text, out val))
            {
                suppyLandStaticDhTextBox.Text = Misc.Economy.SuppyLandStaticDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                suppyLandStaticDhTextBox.Text = Misc.Economy.SuppyLandStaticDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SuppyLandStaticDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SuppyLandStaticDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SuppyLandStaticDh);
            Misc.SetDirty();

            // 文字色を変更する
            suppyLandStaticDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の戦闘時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyLandBattleDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyLandBattleDhTextBox.Text, out val))
            {
                supplyLandBattleDhTextBox.Text = Misc.Economy.SupplyLandBattleDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyLandBattleDhTextBox.Text = Misc.Economy.SupplyLandBattleDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyLandBattleDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyLandBattleDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyLandBattleDh);
            Misc.SetDirty();

            // 文字色を変更する
            supplyLandBattleDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の待機時燃料使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelLandStaticTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(fuelLandStaticTextBox.Text, out val))
            {
                fuelLandStaticTextBox.Text = Misc.Economy.FuelLandStatic.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                fuelLandStaticTextBox.Text = Misc.Economy.FuelLandStatic.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.FuelLandStatic) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.FuelLandStatic = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FuelLandStatic);
            Misc.SetDirty();

            // 文字色を変更する
            fuelLandStaticTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [陸軍の戦闘時燃料使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelLandBattleTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(fuelLandBattleTextBox.Text, out val))
            {
                fuelLandBattleTextBox.Text = Misc.Economy.FuelLandBattle.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                fuelLandBattleTextBox.Text = Misc.Economy.FuelLandBattle.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.FuelLandBattle) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.FuelLandBattle = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FuelLandBattle);
            Misc.SetDirty();

            // 文字色を変更する
            fuelLandBattleTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の待機時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyAirStaticDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyAirStaticDhTextBox.Text, out val))
            {
                supplyAirStaticDhTextBox.Text = Misc.Economy.SupplyAirStaticDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyAirStaticDhTextBox.Text = Misc.Economy.SupplyAirStaticDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyAirStaticDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyAirStaticDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyAirStaticDh);
            Misc.SetDirty();

            // 文字色を変更する
            supplyAirStaticDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の戦闘時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyAirBattleDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyAirBattleDhTextBox.Text, out val))
            {
                supplyAirBattleDhTextBox.Text = Misc.Economy.SupplyAirBattleDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyAirBattleDhTextBox.Text = Misc.Economy.SupplyAirBattleDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyAirBattleDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyAirBattleDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyAirBattleDh);
            Misc.SetDirty();

            // 文字色を変更する
            supplyAirBattleDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍/海軍の待機時燃料使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelAirNavalStaticTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(fuelAirNavalStaticTextBox.Text, out val))
            {
                fuelAirNavalStaticTextBox.Text = Misc.Economy.FuelAirNavalStatic.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                fuelAirNavalStaticTextBox.Text = Misc.Economy.FuelAirNavalStatic.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.FuelAirNavalStatic) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.FuelAirNavalStatic = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FuelAirNavalStatic);
            Misc.SetDirty();

            // 文字色を変更する
            fuelAirNavalStaticTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [空軍の戦闘時燃料使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelAirBattleTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(fuelAirBattleTextBox.Text, out val))
            {
                fuelAirBattleTextBox.Text = Misc.Economy.FuelAirBattle.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                fuelAirBattleTextBox.Text = Misc.Economy.FuelAirBattle.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.FuelAirBattle) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.FuelAirBattle = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FuelAirBattle);
            Misc.SetDirty();

            // 文字色を変更する
            fuelAirBattleTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の待機時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyNavalStaticDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyNavalStaticDhTextBox.Text, out val))
            {
                supplyNavalStaticDhTextBox.Text = Misc.Economy.SupplyNavalStaticDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyNavalStaticDhTextBox.Text = Misc.Economy.SupplyNavalStaticDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyNavalStaticDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyNavalStaticDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyNavalStaticDh);
            Misc.SetDirty();

            // 文字色を変更する
            supplyNavalStaticDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の戦闘時物資使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyNavalBattleDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyNavalBattleDhTextBox.Text, out val))
            {
                supplyNavalBattleDhTextBox.Text = Misc.Economy.SupplyNavalBattleDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                supplyNavalBattleDhTextBox.Text = Misc.Economy.SupplyNavalBattleDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.SupplyNavalBattleDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.SupplyNavalBattleDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SupplyNavalBattleDh);
            Misc.SetDirty();

            // 文字色を変更する
            supplyNavalBattleDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の非移動時燃料使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelNavalNotMovingTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(fuelNavalNotMovingTextBox.Text, out val))
            {
                fuelNavalNotMovingTextBox.Text = Misc.Economy.FuelNavalNotMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                fuelNavalNotMovingTextBox.Text = Misc.Economy.FuelNavalNotMoving.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.FuelNavalNotMoving) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.FuelNavalNotMoving = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FuelNavalNotMoving);
            Misc.SetDirty();

            // 文字色を変更する
            fuelNavalNotMovingTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [海軍の戦闘時燃料使用量補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelNavalBattleTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(fuelNavalBattleTextBox.Text, out val))
            {
                fuelNavalBattleTextBox.Text = Misc.Economy.FuelNavalBattle.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                fuelNavalBattleTextBox.Text = Misc.Economy.FuelNavalBattle.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.FuelNavalBattle) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.FuelNavalBattle = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FuelNavalBattle);
            Misc.SetDirty();

            // 文字色を変更する
            fuelNavalBattleTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [輸送艦の輸送船団への変換比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTpTransportsConversionRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(tpTransportsConversionRatioTextBox.Text, out val))
            {
                tpTransportsConversionRatioTextBox.Text = Misc.Economy.TpTransportsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                tpTransportsConversionRatioTextBox.Text = Misc.Economy.TpTransportsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.TpTransportsConversionRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.TpTransportsConversionRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TpTransportsConversionRatio);
            Misc.SetDirty();

            // 文字色を変更する
            tpTransportsConversionRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [駆逐艦の護衛船団への変換比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDdEscortsConversionRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(ddEscortsConversionRatioTextBox.Text, out val))
            {
                ddEscortsConversionRatioTextBox.Text = Misc.Economy.DdEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                ddEscortsConversionRatioTextBox.Text = Misc.Economy.DdEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DdEscortsConversionRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DdEscortsConversionRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DdEscortsConversionRatio);
            Misc.SetDirty();

            // 文字色を変更する
            ddEscortsConversionRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [軽巡洋艦の護衛船団への変換比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClEscortsConversionRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(clEscortsConversionRatioTextBox.Text, out val))
            {
                clEscortsConversionRatioTextBox.Text = Misc.Economy.ClEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                clEscortsConversionRatioTextBox.Text = Misc.Economy.ClEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ClEscortsConversionRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ClEscortsConversionRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ClEscortsConversionRatio);
            Misc.SetDirty();

            // 文字色を変更する
            clEscortsConversionRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [軽空母の護衛船団への変換比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCvlEscortsConversionRatioTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(cvlEscortsConversionRatioTextBox.Text, out val))
            {
                cvlEscortsConversionRatioTextBox.Text = Misc.Economy.CvlEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                cvlEscortsConversionRatioTextBox.Text = Misc.Economy.CvlEscortsConversionRatio.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.CvlEscortsConversionRatio) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.CvlEscortsConversionRatio = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.CvlEscortsConversionRatio);
            Misc.SetDirty();

            // 文字色を変更する
            cvlEscortsConversionRatioTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [生産ラインの編集]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProductionLineEditComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.ProductionLineEdit ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.ProductionLineEdit))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = productionLineEditComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [生産ラインの編集]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProductionLineEditComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (productionLineEditComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (productionLineEditComboBox.SelectedIndex == 1);
            if (val == Misc.Economy.ProductionLineEdit)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ProductionLineEdit = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ProductionLineEdit);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            productionLineEditComboBox.Refresh();
        }

        /// <summary>
        /// [ユニット改良時のギアリングボーナス減少比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGearingBonusLossUpgradeUnitTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(gearingBonusLossUpgradeUnitTextBox.Text, out val))
            {
                gearingBonusLossUpgradeUnitTextBox.Text = Misc.Economy.GearingBonusLossUpgradeUnit.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                gearingBonusLossUpgradeUnitTextBox.Text = Misc.Economy.GearingBonusLossUpgradeUnit.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.GearingBonusLossUpgradeUnit) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.GearingBonusLossUpgradeUnit = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.GearingBonusLossUpgradeUnit);
            Misc.SetDirty();

            // 文字色を変更する
            gearingBonusLossUpgradeUnitTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [旅団改良時のギアリングボーナス減少比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGearingBonusLossUpgradeBrigadeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(gearingBonusLossUpgradeBrigadeTextBox.Text, out val))
            {
                gearingBonusLossUpgradeBrigadeTextBox.Text = Misc.Economy.GearingBonusLossUpgradeBrigade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                gearingBonusLossUpgradeBrigadeTextBox.Text = Misc.Economy.GearingBonusLossUpgradeBrigade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.GearingBonusLossUpgradeBrigade) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.GearingBonusLossUpgradeBrigade = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.GearingBonusLossUpgradeBrigade);
            Misc.SetDirty();

            // 文字色を変更する
            gearingBonusLossUpgradeBrigadeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [中核州核攻撃時の不満度上昇係数]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDissentNukesTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(dissentNukesTextBox.Text, out val))
            {
                dissentNukesTextBox.Text = Misc.Economy.DissentNukes.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                dissentNukesTextBox.Text = Misc.Economy.DissentNukes.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.DissentNukes) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.DissentNukes = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DissentNukes);
            Misc.SetDirty();

            // 文字色を変更する
            dissentNukesTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [物資/消費財不足時の最大不満度上昇値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxDailyDissentTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxDailyDissentTextBox.Text, out val))
            {
                maxDailyDissentTextBox.Text = Misc.Economy.MaxDailyDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxDailyDissentTextBox.Text = Misc.Economy.MaxDailyDissent.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.MaxDailyDissent) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.MaxDailyDissent = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxDailyDissent);
            Misc.SetDirty();

            // 文字色を変更する
            maxDailyDissentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [核兵器生産補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNukesProductionModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(nukesProductionModifierTextBox.Text, out val))
            {
                nukesProductionModifierTextBox.Text = Misc.Economy.NukesProductionModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                nukesProductionModifierTextBox.Text = Misc.Economy.NukesProductionModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.NukesProductionModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.NukesProductionModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.NukesProductionModifier);
            Misc.SetDirty();

            // 文字色を変更する
            nukesProductionModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [同盟国に対する船団システム]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvoySystemOptionsAlliedComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Economy.ConvoySystemOptionsAllied;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.ConvoySystemOptionsAllied))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = convoySystemOptionsAlliedComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [同盟国に対する船団システム]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvoySystemOptionsAlliedComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (convoySystemOptionsAlliedComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = convoySystemOptionsAlliedComboBox.SelectedIndex;
            if (val == Misc.Economy.ConvoySystemOptionsAllied)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ConvoySystemOptionsAllied = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ConvoySystemOptionsAllied);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            convoySystemOptionsAlliedComboBox.Refresh();
        }

        /// <summary>
        /// [不要な資源/燃料の回収比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceConvoysBackUnneededTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(resourceConvoysBackUnneededTextBox.Text, out val))
            {
                resourceConvoysBackUnneededTextBox.Text = Misc.Economy.ResourceConvoysBackUnneeded.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                resourceConvoysBackUnneededTextBox.Text = Misc.Economy.ResourceConvoysBackUnneeded.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Economy.ResourceConvoysBackUnneeded) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Economy.ResourceConvoysBackUnneeded = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ResourceConvoysBackUnneeded);
            Misc.SetDirty();

            // 文字色を変更する
            resourceConvoysBackUnneededTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 諜報タブ

        /// <summary>
        ///     諜報タブの項目を初期化する
        /// </summary>
        private void InitIntelligenceItems()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                spyMissionDaysDhLabel.Enabled = true;
                increateIntelligenceLevelDaysDhLabel.Enabled = true;
                chanceDetectSpyMissionDhLabel.Enabled = true;
                relationshipsHitDetectedMissionsDhLabel.Enabled = true;
                distanceModifierLabel.Enabled = true;
                distanceModifierNeighboursDhLabel.Enabled = true;
                spyLevelBonusDistanceModifierLabel.Enabled = true;
                spyLevelBonusDistanceModifierAboveTenLabel.Enabled = true;
                spyInformationAccuracyModifierDhLabel.Enabled = true;
                icModifierCostLabel.Enabled = true;
                minIcCostModifierLabel.Enabled = true;
                maxIcCostModifierDhLabel.Enabled = true;
                extraMaintenanceCostAboveTenLabel.Enabled = true;
                extraCostIncreasingAboveTenLabel.Enabled = true;
                showThirdCountrySpyReportsDhLabel.Enabled = true;
                spiesMoneyModifierLabel.Enabled = true;

                spyMissionDaysDhTextBox.Enabled = true;
                increateIntelligenceLevelDaysDhTextBox.Enabled = true;
                chanceDetectSpyMissionDhTextBox.Enabled = true;
                relationshipsHitDetectedMissionsDhTextBox.Enabled = true;
                distanceModifierTextBox.Enabled = true;
                distanceModifierNeighboursDhTextBox.Enabled = true;
                spyLevelBonusDistanceModifierTextBox.Enabled = true;
                spyLevelBonusDistanceModifierAboveTenTextBox.Enabled = true;
                spyInformationAccuracyModifierDhTextBox.Enabled = true;
                icModifierCostTextBox.Enabled = true;
                minIcCostModifierTextBox.Enabled = true;
                maxIcCostModifierDhTextBox.Enabled = true;
                extraMaintenanceCostAboveTenTextBox.Enabled = true;
                extraCostIncreasingAboveTenTextBox.Enabled = true;
                showThirdCountrySpyReportsDhComboBox.Enabled = true;
                spiesMoneyModifierTextBox.Enabled = true;
            }
            else
            {
                spyMissionDaysDhLabel.Enabled = false;
                increateIntelligenceLevelDaysDhLabel.Enabled = false;
                chanceDetectSpyMissionDhLabel.Enabled = false;
                relationshipsHitDetectedMissionsDhLabel.Enabled = false;
                distanceModifierLabel.Enabled = false;
                distanceModifierNeighboursDhLabel.Enabled = false;
                spyLevelBonusDistanceModifierLabel.Enabled = false;
                spyLevelBonusDistanceModifierAboveTenLabel.Enabled = false;
                spyInformationAccuracyModifierDhLabel.Enabled = false;
                icModifierCostLabel.Enabled = false;
                minIcCostModifierLabel.Enabled = false;
                maxIcCostModifierDhLabel.Enabled = false;
                extraMaintenanceCostAboveTenLabel.Enabled = false;
                extraCostIncreasingAboveTenLabel.Enabled = false;
                showThirdCountrySpyReportsDhLabel.Enabled = false;
                spiesMoneyModifierLabel.Enabled = false;

                spyMissionDaysDhTextBox.Enabled = false;
                increateIntelligenceLevelDaysDhTextBox.Enabled = false;
                chanceDetectSpyMissionDhTextBox.Enabled = false;
                relationshipsHitDetectedMissionsDhTextBox.Enabled = false;
                distanceModifierTextBox.Enabled = false;
                distanceModifierNeighboursDhTextBox.Enabled = false;
                spyLevelBonusDistanceModifierTextBox.Enabled = false;
                spyLevelBonusDistanceModifierAboveTenTextBox.Enabled = false;
                spyInformationAccuracyModifierDhTextBox.Enabled = false;
                icModifierCostTextBox.Enabled = false;
                minIcCostModifierTextBox.Enabled = false;
                maxIcCostModifierDhTextBox.Enabled = false;
                extraMaintenanceCostAboveTenTextBox.Enabled = false;
                extraCostIncreasingAboveTenTextBox.Enabled = false;
                showThirdCountrySpyReportsDhComboBox.Enabled = false;
                spiesMoneyModifierTextBox.Enabled = false;
            }
        }

        /// <summary>
        ///     諜報タブの項目を更新する
        /// </summary>
        private void UpdateIntelligenceItems()
        {
            // 編集項目の値を更新する
            if (Game.Type == GameType.DarkestHour)
            {
                spyMissionDaysDhTextBox.Text = Misc.Intelligence.SpyMissionDaysDh.ToString(CultureInfo.InvariantCulture);
                increateIntelligenceLevelDaysDhTextBox.Text = Misc.Intelligence.IncreateIntelligenceLevelDaysDh.ToString(CultureInfo.InvariantCulture);
                chanceDetectSpyMissionDhTextBox.Text = Misc.Intelligence.ChanceDetectSpyMissionDh.ToString(CultureInfo.InvariantCulture);
                relationshipsHitDetectedMissionsDhTextBox.Text = Misc.Intelligence.RelationshipsHitDetectedMissionsDh.ToString(CultureInfo.InvariantCulture);
                distanceModifierTextBox.Text = Misc.Intelligence.DistanceModifier.ToString(CultureInfo.InvariantCulture);
                distanceModifierNeighboursDhTextBox.Text = Misc.Intelligence.DistanceModifierNeighboursDh.ToString(CultureInfo.InvariantCulture);
                spyLevelBonusDistanceModifierTextBox.Text = Misc.Intelligence.SpyLevelBonusDistanceModifier.ToString(CultureInfo.InvariantCulture);
                spyLevelBonusDistanceModifierAboveTenTextBox.Text = Misc.Intelligence.SpyLevelBonusDistanceModifierAboveTen.ToString(CultureInfo.InvariantCulture);
                spyInformationAccuracyModifierDhTextBox.Text = Misc.Intelligence.SpyInformationAccuracyModifierDh.ToString(CultureInfo.InvariantCulture);
                icModifierCostTextBox.Text = Misc.Intelligence.IcModifierCost.ToString(CultureInfo.InvariantCulture);
                minIcCostModifierTextBox.Text = Misc.Intelligence.MinIcCostModifier.ToString(CultureInfo.InvariantCulture);
                maxIcCostModifierDhTextBox.Text = Misc.Intelligence.MaxIcCostModifierDh.ToString(CultureInfo.InvariantCulture);
                extraMaintenanceCostAboveTenTextBox.Text = Misc.Intelligence.ExtraMaintenanceCostAboveTen.ToString(CultureInfo.InvariantCulture);
                extraCostIncreasingAboveTenTextBox.Text = Misc.Intelligence.ExtraCostIncreasingAboveTen.ToString(CultureInfo.InvariantCulture);
                showThirdCountrySpyReportsDhComboBox.SelectedIndex = Misc.Intelligence.ShowThirdCountrySpyReportsDh;
                spiesMoneyModifierTextBox.Text = Misc.Intelligence.SpiesMoneyModifier.ToString(CultureInfo.InvariantCulture);
            }

            // 編集項目の色を更新する
            if (Game.Type == GameType.DarkestHour)
            {
                spyMissionDaysDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyMissionDaysDh) ? Color.Red : SystemColors.WindowText;
                increateIntelligenceLevelDaysDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.IncreateIntelligenceLevelDaysDh) ? Color.Red : SystemColors.WindowText;
                chanceDetectSpyMissionDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceDetectSpyMissionDh) ? Color.Red : SystemColors.WindowText;
                relationshipsHitDetectedMissionsDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.RelationshipsHitDetectedMissionsDh) ? Color.Red : SystemColors.WindowText;
                distanceModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.DistanceModifier) ? Color.Red : SystemColors.WindowText;
                distanceModifierNeighboursDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.DistanceModifierNeighboursDh) ? Color.Red : SystemColors.WindowText;
                spyLevelBonusDistanceModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyLevelBonusDistanceModifier) ? Color.Red : SystemColors.WindowText;
                spyLevelBonusDistanceModifierAboveTenTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyLevelBonusDistanceModifierAboveTen) ? Color.Red : SystemColors.WindowText;
                spyInformationAccuracyModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyInformationAccuracyModifierDh) ? Color.Red : SystemColors.WindowText;
                icModifierCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcModifierCost) ? Color.Red : SystemColors.WindowText;
                minIcCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinIcCostModifier) ? Color.Red : SystemColors.WindowText;
                maxIcCostModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxIcCostModifierDh) ? Color.Red : SystemColors.WindowText;
                extraMaintenanceCostAboveTenTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraMaintenanceCostAboveTen) ? Color.Red : SystemColors.WindowText;
                extraCostIncreasingAboveTenTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraCostIncreasingAboveTen) ? Color.Red : SystemColors.WindowText;
                spiesMoneyModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpiesMoneyModifier) ? Color.Red : SystemColors.WindowText;
            }
        }

        /// <summary>
        /// [諜報任務の間隔]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyMissionDaysDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(spyMissionDaysDhTextBox.Text, out val))
            {
                spyMissionDaysDhTextBox.Text = Misc.Intelligence.SpyMissionDaysDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyMissionDaysDhTextBox.Text = Misc.Intelligence.SpyMissionDaysDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Intelligence.SpyMissionDaysDh)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.SpyMissionDaysDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyMissionDaysDh);
            Misc.SetDirty();

            // 文字色を変更する
            spyMissionDaysDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報レベルの増加間隔]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIncreateIntelligenceLevelDaysDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(increateIntelligenceLevelDaysDhTextBox.Text, out val))
            {
                increateIntelligenceLevelDaysDhTextBox.Text = Misc.Intelligence.IncreateIntelligenceLevelDaysDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                increateIntelligenceLevelDaysDhTextBox.Text = Misc.Intelligence.IncreateIntelligenceLevelDaysDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Intelligence.IncreateIntelligenceLevelDaysDh)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.IncreateIntelligenceLevelDaysDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IncreateIntelligenceLevelDaysDh);
            Misc.SetDirty();

            // 文字色を変更する
            increateIntelligenceLevelDaysDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [国内の諜報活動を発見する確率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChanceDetectSpyMissionDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(chanceDetectSpyMissionDhTextBox.Text, out val))
            {
                chanceDetectSpyMissionDhTextBox.Text = Misc.Intelligence.ChanceDetectSpyMissionDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                chanceDetectSpyMissionDhTextBox.Text = Misc.Intelligence.ChanceDetectSpyMissionDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.ChanceDetectSpyMissionDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.ChanceDetectSpyMissionDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ChanceDetectSpyMissionDh);
            Misc.SetDirty();

            // 文字色を変更する
            chanceDetectSpyMissionDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報任務発覚時の友好度低下量]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationshipsHitDetectedMissionsDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(relationshipsHitDetectedMissionsDhTextBox.Text, out val))
            {
                relationshipsHitDetectedMissionsDhTextBox.Text = Misc.Intelligence.RelationshipsHitDetectedMissionsDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                relationshipsHitDetectedMissionsDhTextBox.Text = Misc.Intelligence.RelationshipsHitDetectedMissionsDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.RelationshipsHitDetectedMissionsDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.RelationshipsHitDetectedMissionsDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RelationshipsHitDetectedMissionsDh);
            Misc.SetDirty();

            // 文字色を変更する
            relationshipsHitDetectedMissionsDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報任務の距離補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDistanceModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(distanceModifierTextBox.Text, out val))
            {
                distanceModifierTextBox.Text = Misc.Intelligence.DistanceModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                distanceModifierTextBox.Text = Misc.Intelligence.DistanceModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.DistanceModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.DistanceModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DistanceModifier);
            Misc.SetDirty();

            // 文字色を変更する
            distanceModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報任務の近隣国補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDistanceModifierNeighboursDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(distanceModifierNeighboursDhTextBox.Text, out val))
            {
                distanceModifierNeighboursDhTextBox.Text = Misc.Intelligence.DistanceModifierNeighboursDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                distanceModifierNeighboursDhTextBox.Text = Misc.Intelligence.DistanceModifierNeighboursDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.DistanceModifierNeighboursDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.DistanceModifierNeighboursDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DistanceModifierNeighboursDh);
            Misc.SetDirty();

            // 文字色を変更する
            distanceModifierNeighboursDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報レベルの距離補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyLevelBonusDistanceModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spyLevelBonusDistanceModifierTextBox.Text, out val))
            {
                spyLevelBonusDistanceModifierTextBox.Text = Misc.Intelligence.SpyLevelBonusDistanceModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyLevelBonusDistanceModifierTextBox.Text = Misc.Intelligence.SpyLevelBonusDistanceModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.SpyLevelBonusDistanceModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.SpyLevelBonusDistanceModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyLevelBonusDistanceModifier);
            Misc.SetDirty();

            // 文字色を変更する
            spyLevelBonusDistanceModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報レベル10超過時の距離補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyLevelBonusDistanceModifierAboveTenTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spyLevelBonusDistanceModifierAboveTenTextBox.Text, out val))
            {
                spyLevelBonusDistanceModifierAboveTenTextBox.Text = Misc.Intelligence.SpyLevelBonusDistanceModifierAboveTen.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyLevelBonusDistanceModifierAboveTenTextBox.Text = Misc.Intelligence.SpyLevelBonusDistanceModifierAboveTen.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.SpyLevelBonusDistanceModifierAboveTen) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.SpyLevelBonusDistanceModifierAboveTen = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyLevelBonusDistanceModifierAboveTen);
            Misc.SetDirty();

            // 文字色を変更する
            spyLevelBonusDistanceModifierAboveTenTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [情報の正確さ補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpyInformationAccuracyModifierDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spyInformationAccuracyModifierDhTextBox.Text, out val))
            {
                spyInformationAccuracyModifierDhTextBox.Text = Misc.Intelligence.SpyInformationAccuracyModifierDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spyInformationAccuracyModifierDhTextBox.Text = Misc.Intelligence.SpyInformationAccuracyModifierDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.SpyInformationAccuracyModifierDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.SpyInformationAccuracyModifierDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpyInformationAccuracyModifierDh);
            Misc.SetDirty();

            // 文字色を変更する
            spyInformationAccuracyModifierDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報コストのIC補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIcModifierCostTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(icModifierCostTextBox.Text, out val))
            {
                icModifierCostTextBox.Text = Misc.Intelligence.IcModifierCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                icModifierCostTextBox.Text = Misc.Intelligence.IcModifierCost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.IcModifierCost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.IcModifierCost = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.IcModifierCost);
            Misc.SetDirty();

            // 文字色を変更する
            icModifierCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報コスト補正の最小IC]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinIcCostModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(minIcCostModifierTextBox.Text, out val))
            {
                minIcCostModifierTextBox.Text = Misc.Intelligence.MinIcCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                minIcCostModifierTextBox.Text = Misc.Intelligence.MinIcCostModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.MinIcCostModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.MinIcCostModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MinIcCostModifier);
            Misc.SetDirty();

            // 文字色を変更する
            minIcCostModifierTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報コスト補正の最大IC]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxIcCostModifierDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxIcCostModifierDhTextBox.Text, out val))
            {
                maxIcCostModifierDhTextBox.Text = Misc.Intelligence.MaxIcCostModifierDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                maxIcCostModifierDhTextBox.Text = Misc.Intelligence.MaxIcCostModifierDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.MaxIcCostModifierDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.MaxIcCostModifierDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MaxIcCostModifierDh);
            Misc.SetDirty();

            // 文字色を変更する
            maxIcCostModifierDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報レベル10超過時追加維持コスト]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtraMaintenanceCostAboveTenTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(extraMaintenanceCostAboveTenTextBox.Text, out val))
            {
                extraMaintenanceCostAboveTenTextBox.Text = Misc.Intelligence.ExtraMaintenanceCostAboveTen.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                extraMaintenanceCostAboveTenTextBox.Text = Misc.Intelligence.ExtraMaintenanceCostAboveTen.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.ExtraMaintenanceCostAboveTen) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.ExtraMaintenanceCostAboveTen = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ExtraMaintenanceCostAboveTen);
            Misc.SetDirty();

            // 文字色を変更する
            extraMaintenanceCostAboveTenTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [諜報レベル10超過時増加コスト]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtraCostIncreasingAboveTenTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(extraCostIncreasingAboveTenTextBox.Text, out val))
            {
                extraCostIncreasingAboveTenTextBox.Text = Misc.Intelligence.ExtraCostIncreasingAboveTen.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                extraCostIncreasingAboveTenTextBox.Text = Misc.Intelligence.ExtraCostIncreasingAboveTen.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.ExtraCostIncreasingAboveTen) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.ExtraCostIncreasingAboveTen = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ExtraCostIncreasingAboveTen);
            Misc.SetDirty();

            // 文字色を変更する
            extraCostIncreasingAboveTenTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [第三国の諜報活動を報告するか]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowThirdCountrySpyReportsDhComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Intelligence.ShowThirdCountrySpyReportsDh;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.ShowThirdCountrySpyReportsDh))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = showThirdCountrySpyReportsDhComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [第三国の諜報活動を報告するか]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowThirdCountrySpyReportsDhComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (showThirdCountrySpyReportsDhComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = showThirdCountrySpyReportsDhComboBox.SelectedIndex;
            if (val == Misc.Intelligence.ShowThirdCountrySpyReportsDh)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.ShowThirdCountrySpyReportsDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ShowThirdCountrySpyReportsDh);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            showThirdCountrySpyReportsDhComboBox.Refresh();
        }

        /// <summary>
        /// [諜報資金割り当て補正]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpiesMoneyModifierTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(spiesMoneyModifierTextBox.Text, out val))
            {
                spiesMoneyModifierTextBox.Text = Misc.Intelligence.SpiesMoneyModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                spiesMoneyModifierTextBox.Text = Misc.Intelligence.SpiesMoneyModifier.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Intelligence.SpiesMoneyModifier) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Intelligence.SpiesMoneyModifier = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.SpiesMoneyModifier);
            Misc.SetDirty();

            // 文字色を変更する
            spiesMoneyModifierTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region 外交タブ

        /// <summary>
        ///     外交タブの項目を初期化する
        /// </summary>
        private void InitDiplomacyItems()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                daysBetweenDiplomaticMissionsLabel.Enabled = true;
                timeBetweenSliderChangesDhLabel.Enabled = true;
                requirementAffectSliderDhLabel.Enabled = true;
                useMinisterPersonalityReplacingLabel.Enabled = true;
                relationshipHitCancelTradeLabel.Enabled = true;
                relationshipHitCancelPermanentTradeLabel.Enabled = true;
                puppetsJoinMastersAllianceLabel.Enabled = true;
                mastersBecomePuppetsPuppetsLabel.Enabled = true;
                allowManualClaimsChangeLabel.Enabled = true;
                belligerenceClaimedProvinceLabel.Enabled = true;
                belligerenceClaimsRemovalLabel.Enabled = true;
                joinAutomaticallyAllesAxisLabel.Enabled = true;
                allowChangeHosHogLabel.Enabled = true;
                changeTagCoupLabel.Enabled = true;
                filterReleaseCountriesLabel.Enabled = true;

                daysBetweenDiplomaticMissionsTextBox.Enabled = true;
                timeBetweenSliderChangesDhTextBox.Enabled = true;
                requirementAffectSliderDhTextBox.Enabled = true;
                useMinisterPersonalityReplacingComboBox.Enabled = true;
                relationshipHitCancelTradeTextBox.Enabled = true;
                relationshipHitCancelPermanentTradeTextBox.Enabled = true;
                puppetsJoinMastersAllianceComboBox.Enabled = true;
                mastersBecomePuppetsPuppetsComboBox.Enabled = true;
                allowManualClaimsChangeComboBox.Enabled = true;
                belligerenceClaimedProvinceTextBox.Enabled = true;
                belligerenceClaimsRemovalTextBox.Enabled = true;
                joinAutomaticallyAllesAxisComboBox.Enabled = true;
                allowChangeHosHogComboBox.Enabled = true;
                changeTagCoupComboBox.Enabled = true;
                filterReleaseCountriesComboBox.Enabled = true;
            }
            else
            {
                daysBetweenDiplomaticMissionsLabel.Enabled = false;
                timeBetweenSliderChangesDhLabel.Enabled = false;
                requirementAffectSliderDhLabel.Enabled = false;
                useMinisterPersonalityReplacingLabel.Enabled = false;
                relationshipHitCancelTradeLabel.Enabled = false;
                relationshipHitCancelPermanentTradeLabel.Enabled = false;
                puppetsJoinMastersAllianceLabel.Enabled = false;
                mastersBecomePuppetsPuppetsLabel.Enabled = false;
                allowManualClaimsChangeLabel.Enabled = false;
                belligerenceClaimedProvinceLabel.Enabled = false;
                belligerenceClaimsRemovalLabel.Enabled = false;
                joinAutomaticallyAllesAxisLabel.Enabled = false;
                allowChangeHosHogLabel.Enabled = false;
                changeTagCoupLabel.Enabled = false;
                filterReleaseCountriesLabel.Enabled = false;

                daysBetweenDiplomaticMissionsTextBox.Enabled = false;
                timeBetweenSliderChangesDhTextBox.Enabled = false;
                requirementAffectSliderDhTextBox.Enabled = false;
                useMinisterPersonalityReplacingComboBox.Enabled = false;
                relationshipHitCancelTradeTextBox.Enabled = false;
                relationshipHitCancelPermanentTradeTextBox.Enabled = false;
                puppetsJoinMastersAllianceComboBox.Enabled = false;
                mastersBecomePuppetsPuppetsComboBox.Enabled = false;
                allowManualClaimsChangeComboBox.Enabled = false;
                belligerenceClaimedProvinceTextBox.Enabled = false;
                belligerenceClaimsRemovalTextBox.Enabled = false;
                joinAutomaticallyAllesAxisComboBox.Enabled = false;
                allowChangeHosHogComboBox.Enabled = false;
                changeTagCoupComboBox.Enabled = false;
                filterReleaseCountriesComboBox.Enabled = false;
            }
        }

        /// <summary>
        ///     外交タブの項目を更新する
        /// </summary>
        private void UpdateDiplomacyItems()
        {
            // 編集項目の値を更新する
            if (Game.Type == GameType.DarkestHour)
            {
                daysBetweenDiplomaticMissionsTextBox.Text = Misc.Diplomacy.DaysBetweenDiplomaticMissions.ToString(CultureInfo.InvariantCulture);
                timeBetweenSliderChangesDhTextBox.Text = Misc.Diplomacy.TimeBetweenSliderChangesDh.ToString(CultureInfo.InvariantCulture);
                requirementAffectSliderDhTextBox.Text = Misc.Diplomacy.RequirementAffectSliderDh.ToString(CultureInfo.InvariantCulture);
                useMinisterPersonalityReplacingComboBox.SelectedIndex = Misc.Diplomacy.UseMinisterPersonalityReplacing ? 1 : 0;
                relationshipHitCancelTradeTextBox.Text = Misc.Diplomacy.RelationshipHitCancelTrade.ToString(CultureInfo.InvariantCulture);
                relationshipHitCancelPermanentTradeTextBox.Text = Misc.Diplomacy.RelationshipHitCancelPermanentTrade.ToString(CultureInfo.InvariantCulture);
                puppetsJoinMastersAllianceComboBox.SelectedIndex = Misc.Diplomacy.PuppetsJoinMastersAlliance ? 1 : 0;
                mastersBecomePuppetsPuppetsComboBox.SelectedIndex = Misc.Diplomacy.MastersBecomePuppetsPuppets ? 1 : 0;
                allowManualClaimsChangeComboBox.SelectedIndex = Misc.Diplomacy.AllowManualClaimsChange ? 1 : 0;
                belligerenceClaimedProvinceTextBox.Text = Misc.Diplomacy.BelligerenceClaimedProvince.ToString(CultureInfo.InvariantCulture);
                belligerenceClaimsRemovalTextBox.Text = Misc.Diplomacy.BelligerenceClaimsRemoval.ToString(CultureInfo.InvariantCulture);
                joinAutomaticallyAllesAxisComboBox.SelectedIndex = Misc.Diplomacy.JoinAutomaticallyAllesAxis ? 1 : 0;
                allowChangeHosHogComboBox.SelectedIndex = Misc.Diplomacy.AllowChangeHosHog;
                changeTagCoupComboBox.SelectedIndex = Misc.Diplomacy.ChangeTagCoup ? 1 : 0;
                filterReleaseCountriesComboBox.SelectedIndex = Misc.Diplomacy.FilterReleaseCountries;
            }

            // 編集項目の色を更新する
            if (Game.Type == GameType.DarkestHour)
            {
                daysBetweenDiplomaticMissionsTextBox.ForeColor = Misc.IsDirty(MiscItemId.DaysBetweenDiplomaticMissions) ? Color.Red : SystemColors.WindowText;
                timeBetweenSliderChangesDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.TimeBetweenSliderChangesDh) ? Color.Red : SystemColors.WindowText;
                requirementAffectSliderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.RequirementAffectSliderDh) ? Color.Red : SystemColors.WindowText;
                relationshipHitCancelTradeTextBox.ForeColor = Misc.IsDirty(MiscItemId.RelationshipHitCancelTrade) ? Color.Red : SystemColors.WindowText;
                relationshipHitCancelPermanentTradeTextBox.ForeColor = Misc.IsDirty(MiscItemId.RelationshipHitCancelPermanentTrade) ? Color.Red : SystemColors.WindowText;
                belligerenceClaimedProvinceTextBox.ForeColor = Misc.IsDirty(MiscItemId.BelligerenceClaimedProvince) ? Color.Red : SystemColors.WindowText;
                belligerenceClaimsRemovalTextBox.ForeColor = Misc.IsDirty(MiscItemId.BelligerenceClaimsRemoval) ? Color.Red : SystemColors.WindowText;
            }
        }

        /// <summary>
        /// [外交官派遣間隔]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDaysBetweenDiplomaticMissionsTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(daysBetweenDiplomaticMissionsTextBox.Text, out val))
            {
                daysBetweenDiplomaticMissionsTextBox.Text = Misc.Diplomacy.DaysBetweenDiplomaticMissions.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                daysBetweenDiplomaticMissionsTextBox.Text = Misc.Diplomacy.DaysBetweenDiplomaticMissions.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Diplomacy.DaysBetweenDiplomaticMissions)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.DaysBetweenDiplomaticMissions = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.DaysBetweenDiplomaticMissions);
            Misc.SetDirty();

            // 文字色を変更する
            daysBetweenDiplomaticMissionsTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [スライダー移動の間隔]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeBetweenSliderChangesDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!int.TryParse(timeBetweenSliderChangesDhTextBox.Text, out val))
            {
                timeBetweenSliderChangesDhTextBox.Text = Misc.Diplomacy.TimeBetweenSliderChangesDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                timeBetweenSliderChangesDhTextBox.Text = Misc.Diplomacy.TimeBetweenSliderChangesDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (val == Misc.Diplomacy.TimeBetweenSliderChangesDh)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.TimeBetweenSliderChangesDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.TimeBetweenSliderChangesDh);
            Misc.SetDirty();

            // 文字色を変更する
            timeBetweenSliderChangesDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [政策スライダーに影響を与えるためのIC比率]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRequirementAffectSliderDhTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(requirementAffectSliderDhTextBox.Text, out val))
            {
                requirementAffectSliderDhTextBox.Text = Misc.Diplomacy.RequirementAffectSliderDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                requirementAffectSliderDhTextBox.Text = Misc.Diplomacy.RequirementAffectSliderDh.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Diplomacy.RequirementAffectSliderDh) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.RequirementAffectSliderDh = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RequirementAffectSliderDh);
            Misc.SetDirty();

            // 文字色を変更する
            requirementAffectSliderDhTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [閣僚交代時に閣僚特性を適用する]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUseMinisterPersonalityReplacingComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.UseMinisterPersonalityReplacing ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.UseMinisterPersonalityReplacing))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = useMinisterPersonalityReplacingComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [閣僚交代時に閣僚特性を適用する]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUseMinisterPersonalityReplacingComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (useMinisterPersonalityReplacingComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (useMinisterPersonalityReplacingComboBox.SelectedIndex == 1);
            if (val == Misc.Diplomacy.UseMinisterPersonalityReplacing)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.UseMinisterPersonalityReplacing = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.UseMinisterPersonalityReplacing);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            useMinisterPersonalityReplacingComboBox.Refresh();
        }

        /// <summary>
        /// [貿易キャンセル時の友好度低下]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationshipHitCancelTradeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(relationshipHitCancelTradeTextBox.Text, out val))
            {
                relationshipHitCancelTradeTextBox.Text = Misc.Diplomacy.RelationshipHitCancelTrade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0 || val > 400)
            {
                relationshipHitCancelTradeTextBox.Text = Misc.Diplomacy.RelationshipHitCancelTrade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Diplomacy.RelationshipHitCancelTrade) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.RelationshipHitCancelTrade = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RelationshipHitCancelTrade);
            Misc.SetDirty();

            // 文字色を変更する
            relationshipHitCancelTradeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [永久貿易キャンセル時の友好度低下]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationshipHitCancelPermanentTradeTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(relationshipHitCancelPermanentTradeTextBox.Text, out val))
            {
                relationshipHitCancelPermanentTradeTextBox.Text = Misc.Diplomacy.RelationshipHitCancelPermanentTrade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if ((val < 0 && Math.Abs(val - 0.1) > 0.00005) || val > 100)
            {
                relationshipHitCancelPermanentTradeTextBox.Text = Misc.Diplomacy.RelationshipHitCancelPermanentTrade.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Diplomacy.RelationshipHitCancelPermanentTrade) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.RelationshipHitCancelPermanentTrade = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.RelationshipHitCancelPermanentTrade);
            Misc.SetDirty();

            // 文字色を変更する
            relationshipHitCancelPermanentTradeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [属国が宗主国の同盟に強制参加する]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPuppetsJoinMastersAllianceComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.PuppetsJoinMastersAlliance ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.PuppetsJoinMastersAlliance))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = puppetsJoinMastersAllianceComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [属国が宗主国の同盟に強制参加する]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPuppetsJoinMastersAllianceComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (puppetsJoinMastersAllianceComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (puppetsJoinMastersAllianceComboBox.SelectedIndex == 1);
            if (val == Misc.Diplomacy.PuppetsJoinMastersAlliance)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.PuppetsJoinMastersAlliance = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.PuppetsJoinMastersAlliance);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            puppetsJoinMastersAllianceComboBox.Refresh();
        }

        /// <summary>
        ///     [属国の属国が設立できるか]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMastersBecomePuppetsPuppetsComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.MastersBecomePuppetsPuppets ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.MastersBecomePuppetsPuppets))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = mastersBecomePuppetsPuppetsComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [属国の属国が設立できるか]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMastersBecomePuppetsPuppetsComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (mastersBecomePuppetsPuppetsComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (mastersBecomePuppetsPuppetsComboBox.SelectedIndex == 1);
            if (val == Misc.Diplomacy.MastersBecomePuppetsPuppets)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.MastersBecomePuppetsPuppets = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.MastersBecomePuppetsPuppets);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            mastersBecomePuppetsPuppetsComboBox.Refresh();
        }

        /// <summary>
        ///     [領有権主張の変更]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowManualClaimsChangeComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.AllowManualClaimsChange ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.AllowManualClaimsChange))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = allowManualClaimsChangeComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [領有権主張の変更]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowManualClaimsChangeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (allowManualClaimsChangeComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (allowManualClaimsChangeComboBox.SelectedIndex == 1);
            if (val == Misc.Diplomacy.AllowManualClaimsChange)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.AllowManualClaimsChange = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.AllowManualClaimsChange);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            allowManualClaimsChangeComboBox.Refresh();
        }

        /// <summary>
        /// [領有権主張時の好戦性上昇値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBelligerenceClaimedProvinceTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(belligerenceClaimedProvinceTextBox.Text, out val))
            {
                belligerenceClaimedProvinceTextBox.Text = Misc.Diplomacy.BelligerenceClaimedProvince.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val < 0)
            {
                belligerenceClaimedProvinceTextBox.Text = Misc.Diplomacy.BelligerenceClaimedProvince.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Diplomacy.BelligerenceClaimedProvince) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.BelligerenceClaimedProvince = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.BelligerenceClaimedProvince);
            Misc.SetDirty();

            // 文字色を変更する
            belligerenceClaimedProvinceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// [領有権撤回時の好戦性減少値]テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBelligerenceClaimsRemovalTextBoxValidated(object sender, EventArgs e)
        {
            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(belligerenceClaimsRemovalTextBox.Text, out val))
            {
                belligerenceClaimsRemovalTextBox.Text = Misc.Diplomacy.BelligerenceClaimsRemoval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 設定範囲外の値ならば戻す
            if (val > 0)
            {
                belligerenceClaimsRemovalTextBox.Text = Misc.Diplomacy.BelligerenceClaimsRemoval.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - Misc.Diplomacy.BelligerenceClaimsRemoval) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.BelligerenceClaimsRemoval = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.BelligerenceClaimsRemoval);
            Misc.SetDirty();

            // 文字色を変更する
            belligerenceClaimsRemovalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     [宣戦布告された時に対抗陣営へ自動加盟]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnJoinAutomaticallyAllesAxisComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.JoinAutomaticallyAllesAxis ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.JoinAutomaticallyAllesAxis))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = joinAutomaticallyAllesAxisComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [宣戦布告された時に対抗陣営へ自動加盟]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnJoinAutomaticallyAllesAxisComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (joinAutomaticallyAllesAxisComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (joinAutomaticallyAllesAxisComboBox.SelectedIndex == 1);
            if (val == Misc.Diplomacy.JoinAutomaticallyAllesAxis)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.JoinAutomaticallyAllesAxis = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.JoinAutomaticallyAllesAxis);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            joinAutomaticallyAllesAxisComboBox.Refresh();
        }

        /// <summary>
        ///     [国家元首/政府首班の交代]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowChangeHosHogComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.AllowChangeHosHog;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.AllowChangeHosHog))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = allowChangeHosHogComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [国家元首/政府首班の交代]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowChangeHosHogComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (allowChangeHosHogComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = allowChangeHosHogComboBox.SelectedIndex;
            if (val == Misc.Diplomacy.AllowChangeHosHog)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.AllowChangeHosHog = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.AllowChangeHosHog);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            allowChangeHosHogComboBox.Refresh();
        }

        /// <summary>
        ///     [クーデター発生時に兄弟国へ変更]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeTagCoupComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.ChangeTagCoup ? 1 : 0;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.ChangeTagCoup))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = changeTagCoupComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [クーデター発生時に兄弟国へ変更]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnJChangeTagCoupComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (changeTagCoupComboBox.SelectedIndex == -1)
            {
                return;
            }
            bool val = (changeTagCoupComboBox.SelectedIndex == 1);
            if (val == Misc.Diplomacy.ChangeTagCoup)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.ChangeTagCoup = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.ChangeTagCoup);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            changeTagCoupComboBox.Refresh();
        }

        /// <summary>
        ///     [独立可能国設定]コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilterReleaseCountriesComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int val = Misc.Diplomacy.FilterReleaseCountries;
            if ((e.Index == val) && Misc.IsDirty(MiscItemId.FilterReleaseCountries))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = filterReleaseCountriesComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     [独立可能国設定]変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilterReleaseCountriesComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 値に変化がなければ何もしない
            if (filterReleaseCountriesComboBox.SelectedIndex == -1)
            {
                return;
            }
            int val = filterReleaseCountriesComboBox.SelectedIndex;
            if (val == Misc.Diplomacy.FilterReleaseCountries)
            {
                return;
            }

            // 値を更新する
            Misc.Diplomacy.FilterReleaseCountries = val;

            // 編集済みフラグを設定する
            Misc.SetDirty(MiscItemId.FilterReleaseCountries);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            filterReleaseCountriesComboBox.Refresh();
        }

        #endregion
    }
}