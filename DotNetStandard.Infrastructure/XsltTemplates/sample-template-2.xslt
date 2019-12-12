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
        Fund Lending Tool has identified below loans which are 'Open' for <xsl:value-of select="MaturingLoanAlertViewModel/DayCountLimit"/> or more days.
        <br/><br/>
        Request you to please 'Close' the loan.
        <p>
          <span>
            Click the following link to view the maturing loans:
            <a>
              <xsl:attribute name="href">
                <xsl:value-of select="MaturingLoanAlertViewModel/ApplicationUrl"/>
              </xsl:attribute>
              <xsl:value-of select="MaturingLoanAlertViewModel/ApplicationUrl"/>
            </a>
          </span>
        </p>
      </p>
      <p>
        <table cellspacing="0">
          <tbody>
            <tr>
              <th class="center">Open Date</th>
              <th class="left">Borrower</th>
              <th class="right">Loan Amount</th>
              <th class="left">Lender</th>
              <th class="right">Current Rate</th>
              <th class="right">Day Count</th>
              <th class="right">Est. Payback (Today)</th>
              <th class="left">Status</th>
              <th class="left">Wire Id</th>
              <th class="left">Initiated By</th>
              <th class="left">Approvers</th>
            </tr>
            <xsl:for-each select="MaturingLoanAlertViewModel/LoanViewModels/LoanWorkflowAlertViewModel">
              <tr>
                <td class="center">
                  <xsl:value-of select="StartDate"/>
                </td>
                <td class="left">
                  <xsl:value-of select="BorrowingFund"/>
                </td>
                <td class="right">
                  $<xsl:value-of select='format-number(LoanAmount, "###,###.00")' />
                </td>
                <td class="left">
                  <xsl:value-of select="LenderCode"/>
                </td>
                <td class="right">
                  <xsl:value-of select='format-number(LendingRate, "###,###.00")' />%
                </td>
                <td class="righ">
                  <xsl:value-of select="DayCount"/>
                </td>
                <td class="right">
                  $<xsl:value-of select='format-number(EstimatedPayback, "###,###.00")' />
                </td>
                <td class="left" style="width:300px !important">
                  <xsl:value-of select="Status"/>
                </td>
                <td class="left">
                  <xsl:value-of select='WireId' />
                </td>
                <td class="left" style="width:400px !important">
                  <xsl:value-of select="InitiatedBy"/>
                </td>
                <td class="left" style="width:450px !important">
                  <xsl:for-each select="ApprovalStatuses/ApprovalStatus">
                    <div class="row">
                      <span class="labelDiv">
                        <xsl:value-of select="ActionLabel"/>
                      </span>
                      <span class="valueDiv">
                        <xsl:value-of select="DisplayName"/>
                      </span>
                    </div>
                  </xsl:for-each>
                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </p>
    </html>
  </xsl:template>
</xsl:stylesheet>
