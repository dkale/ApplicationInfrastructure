<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#160;">
]>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <style>
        table { border: 1px solid #C1C3D1; }
        body { font-family: "Helvetica Neue",Helvetica,Arial,sans-serif; font-size: 12px; }
        th {
        color: #FFFFFF; background: #696969; padding: 10px; height; 30px; vertical-align: middle;font-weight: bold; font-size: 13px;
        }
        td {
        color: #666B85; padding: 10px; height: 30px; vertical-align: middle;  font-size: 12px;
        }
        .left { text-align: left }
        .center { text-align: center }
        .right { text-align: right }
        .row { display: flex; }
        .labelDiv { color: #696969; font-weight: bold; }
        .valueDiv { color: #696969; }
      </style>
      <p>
        Hi,
        <br/><br/>
        <span>
          The Fund Lending tool has identified below funds requiring loans as per the cash projections for date <xsl:value-of select='ProposedLoansAlertViewModel/RunDate' />.
          <br/><br/>
          Kindly initiate loans using the calculated proposed loan amount as below.
          <p>
            <span>
              Click the following link to view the cash projections and initiate loans:
              <a>
                <xsl:attribute name="href">
                  <xsl:value-of select="ProposedLoansAlertViewModel/ApplicationUrl"/>
                </xsl:attribute>
                <xsl:value-of select="ProposedLoansAlertViewModel/ApplicationUrl"/>
              </a>
            </span>
          </p>
        </span>
      </p>
      <table cellspacing="0">
        <tbody>
          <tr>
            <th>Portfolio</th>
            <th>Prior EOD Cash</th>
            <th>Inflow</th>
            <th>Outflow</th>
            <th>Cash Projection</th>
            <th>Min Cash Requirement</th>
            <th>Prior EOD NAV</th>
            <th>Proposed Loan Amount</th>
          </tr>
          <xsl:for-each select="ProposedLoansAlertViewModel/CashProjectionViewModels/CashProjectionViewModel">
            <tr>
              <td class="center">
                <xsl:value-of select="Portfolio"/>
              </td>
              <td class="right">
                $<xsl:value-of select='format-number(EodCash, "###,##0.00")' />
              </td>
              <td class="right">
                $<xsl:value-of select='format-number(Inflow, "###,##0.00")' />
              </td>
              <td class="right">
                $<xsl:value-of select='format-number(Outflow, "###,##0.00")' />
              </td>
              <td class="right">
                $<xsl:value-of select='format-number(CashProjection, "###,##0.00")' />
              </td>
              <td class="right">
                <xsl:if test="not(MinCashRequirement)">
                  $0.00
                </xsl:if>
                <xsl:if test="MinCashRequirement">
                  $<xsl:value-of select='format-number(MinCashRequirement, "###,##0.00")' />
                </xsl:if>
              </td>
              <td class="right">
                $<xsl:value-of select='format-number(CurrentNav, "###,##0.00")' />
              </td>
              <td class="right">
                $<xsl:value-of select='format-number(ProposedLoanAmount, "###,##0.00")' />
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
    </html>
  </xsl:template>
</xsl:stylesheet>
