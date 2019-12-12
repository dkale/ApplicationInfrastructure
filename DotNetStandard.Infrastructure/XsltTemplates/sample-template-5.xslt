<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#160;">
]>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <style>
        td {
        font-family: Arial;
        font-size: 20px;
        padding: 4px;
        vertical-align: top;
        }
        .loan-label {
        width:450px;
        }
        .loan-value {
        width:270px;
        text-align:right;
        }
        .wins-span {
        width: 200px;
        display: block;
        text-align: center;
        }
        .page-header {
        padding: 30px 0;
        text-align: center;
        font-weight:bold;
        font-size: 25px;
        font-family: Arial;
        }
        .logo {
        position: absolute;
        left: 25px;
        top: 15px;
        height: 75px;
        }
        .body-content {
        padding: 20px 0px;
        }
      </style>
      <body style="font-weight:bold !important">
        <div class="page-header">
          <img style="height: 75px;" align="center" class="logo">
            <xsl:attribute name="src">
              <xsl:value-of select="LoanTicketViewModel/AqrLogoBase64String"/>
            </xsl:attribute>
            <span style="color:#19a0dd">Closing Ticket</span>
          </img>
        </div>
        <hr size="2px" color="#19a0dd" />
        <div class="body-content" style="width:1000px">
          <table style="display:inline-table;">
            <tbody>
              <tr>
                <td style="width: 250px;">Borrowing Fund (CREDIT):</td>
                <td style="width: 90px;">
                  <xsl:value-of select="LoanTicketViewModel/BorrowingFund"/>
                </td>
                <td style="width: 250px;">
                  <span class="wins-span">
                    <xsl:value-of select="LoanTicketViewModel/BorrowerWinsCashAccount"/>
                  </span>
                </td>
              </tr>
              <tr>
                <td style="width: 250px;">Lending Fund (DEBIT):</td>
                <td style="width: 90px;">
                  <xsl:value-of select="LoanTicketViewModel/LendingFund"/>
                </td>
                <td style="width: 250px;">
                  <span class="wins-span">
                    <xsl:value-of select="LoanTicketViewModel/LenderWinsCashAccount"/>
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
          <table style="display:inline-table;float:right;">
            <tbody>
              <tr>
                <td style="width: 140px;">Trade Date:</td>
                <td style="width: 75px;">
                  <xsl:value-of select="LoanTicketViewModel/TradeDate"/>
                </td>
              </tr>
              <tr>
                <td style="width: 140px;">Maturity Date:</td>
                <td style="width: 75px;">
                  <xsl:value-of select="LoanTicketViewModel/MaturityDate"/>
                </td>
              </tr>
            </tbody>
          </table>
          <br />
          <br />
          <br />
          <br />
          <table>
            <tbody>
              <tr>
                <td class="loan-label">Borrowing Amount:</td>
                <td class="loan-value"></td>
                <td class="loan-value">
                  $<xsl:value-of select='format-number(LoanTicketViewModel/LoanAmount, "###,###.00")' />
                </td>
              </tr>
              <tr style='height:10px'>
                <td colspan='3'></td>
              </tr>
              <!--Lending Rates Count: <xsl:value-of select="count(LoanTicketViewModel/LendingRates/LendingRate)"/>
              <xsl:if test="count(LoanTicketViewModel/LendingRates/LendingRate) = 0">
                Empty Lending Rates Exist
              </xsl:if>-->
              <xsl:if test="count(LoanTicketViewModel/LendingRates/LendingRate) != 0">
                <tr>
                  <td class="loan-value"></td>
                  <td class="loan-value">Rate</td>
                  <td class="loan-value">Interest Accrued</td>
                </tr>
              </xsl:if>
              <xsl:for-each select="LoanTicketViewModel/LendingRates/LendingRate">
                <xsl:variable name="index" select="position()"/>
                <tr>
                  <td class="loan-label">
                    Interfund Lending Rate Day <xsl:value-of select="$index" />:
                  </td>
                  <td class="loan-value">
                    <xsl:value-of select='format-number(InterfundLendingRate, "###,###.0000")' />%
                  </td>
                  <td class="loan-value">
                    $<xsl:value-of select='format-number(InterestAccrued, "###,##0.00")' />
                  </td>
                </tr>
              </xsl:for-each>
              <tr style='height:10px'>
                <td colspan='2'></td>
              </tr>
              <tr>
                <td class="loan-label">Borrowed Interest:</td>
                <td class="loan-value"></td>
                <td class="loan-value">
                  $<xsl:value-of select='format-number(LoanTicketViewModel/EstimatedPayback - LoanTicketViewModel/LoanAmount, "###,##0.00")' />
                </td>
              </tr>
              <tr>
                <td class="loan-label">Payback Amount:</td>
                <td class="loan-value"></td>
                <td class="loan-value">
                  $<xsl:value-of select='format-number(LoanTicketViewModel/EstimatedPayback, "###,###.00")' />
                </td>
              </tr>
            </tbody>
          </table>
          <br />
          <br />
          <br />
          <br />
          <table>
            <tbody>
              <xsl:for-each select="LoanTicketViewModel/AuthorizedSigners/AuthorizedSigner">
                <tr style="height: 18px;">
                  <td style="width: 250px; height: 18px;">Authorized Signer:</td>
                  <td style="width: 500px; height: 18px;">
                    <xsl:value-of select="UserDisplayName"/>
                  </td>
                </tr>
                <tr style="height: 100px;">
                  <td style="width: 250px; height: 72px;vertical-align: top;">Authorized Signature:</td>
                  <td style="width: 500px; height: 72px;vertical-align: top;">
                    <img height="100px">
                      <xsl:attribute name="src">
                        <xsl:value-of select="UserSignatureBase64Image"/>
                      </xsl:attribute>
                    </img>
                  </td>
                </tr>
                <tr style="height: 20px;">
                  <td colspan="2"></td>
                </tr>
              </xsl:for-each>
            </tbody>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
