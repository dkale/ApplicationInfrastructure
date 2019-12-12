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
        <p>
          Hi,
        </p>
        <p>
          <xsl:if test="LoanWorkflowAlertViewModel/Status = 'Pending1stApproval'">
            <span>
              A loan has been initiated through Fund Lending application and awaiting for an approval.
            </span>
          </xsl:if>
          <xsl:if test="LoanWorkflowAlertViewModel/Status = 'Pending2ndApproval'">
            <span>
              A loan has been approved through Fund Lending application and is awaiting for second approval.
            </span>
          </xsl:if>
          <xsl:if test="LoanWorkflowAlertViewModel/Status = 'Open'">
            <span>
              A loan has been approved through Fund Lending application and is now Open.
            </span>
          </xsl:if>
          <xsl:if test="LoanWorkflowAlertViewModel/Status = 'Rejected'">
            <span>
              A loan was rejected through Fund Lending application.
            </span>
          </xsl:if>
          <xsl:if test="LoanWorkflowAlertViewModel/Status = 'Closed'">
            <span>
              A loan has been closed through Fund Lending application.
            </span>
          </xsl:if>
          <xsl:if test="LoanWorkflowAlertViewModel/Comment and string-length(LoanWorkflowAlertViewModel/Comment) > 0">
            <p>
              <span class="labelDiv">
                Approver's comment:
              </span>
              <span class="valueDiv">
                <xsl:value-of select="LoanWorkflowAlertViewModel/Comment"/>
              </span>
            </p>
          </xsl:if>
        </p>
        <p>
          <span>
            Click the following link to view the loan details:
            <a>
              <xsl:attribute name="href">
                <xsl:value-of select="LoanWorkflowAlertViewModel/LoanUrl"/>
              </xsl:attribute>
              <xsl:value-of select="LoanWorkflowAlertViewModel/LoanUrl"/>
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
              <xsl:if test="LoanWorkflowAlertViewModel.DayCount != 0">
                <th class="right">Day Count</th>
              </xsl:if>
              <xsl:if test="LoanWorkflowAlertViewModel.DayCount != 0">
                <th class="right">Est. Payback (Today)</th>
              </xsl:if>
              <th class="left">Status</th>
              <xsl:if test="LoanWorkflowAlertViewModel/Status = 'Open' or LoanWorkflowAlertViewModel/Status = 'Closed'">
                <th class="left">Wire Id</th>
              </xsl:if>
              <th class="left">Initiated By</th>
              <xsl:if test="count(LoanWorkflowAlertViewModel/ApprovalStatuses/ApprovalStatus) != 0">
                <th class="left">Approvers</th>
              </xsl:if>
            </tr>
            <tr>
              <td class="center">
                <xsl:value-of select="LoanWorkflowAlertViewModel/StartDate"/>
              </td>
              <td class="left">
                <xsl:value-of select="LoanWorkflowAlertViewModel/BorrowingFund"/>
              </td>
              <td class="right">
                $<xsl:value-of select='format-number(LoanWorkflowAlertViewModel/LoanAmount, "###,###.00")' />
              </td>
              <td class="left">
                <xsl:value-of select="LoanWorkflowAlertViewModel/LenderCode"/>
              </td>
              <td class="right">
                <xsl:value-of select='format-number(LoanWorkflowAlertViewModel/LendingRate, "###,###.00")' />%
              </td>
              <xsl:if test="LoanWorkflowAlertViewModel.DayCount != 0">
                <td class="right">
                  <xsl:value-of select="LoanWorkflowAlertViewModel/DayCount"/>
                </td>
              </xsl:if>
              <xsl:if test="LoanWorkflowAlertViewModel.DayCount != 0">
                <td class="right">
                  $<xsl:value-of select='format-number(LoanWorkflowAlertViewModel/EstimatedPayback, "###,###.00")' />
                </td>
              </xsl:if>
              <td class="left">
                <xsl:value-of select="LoanWorkflowAlertViewModel/Status"/>
              </td>
              <xsl:if test="LoanWorkflowAlertViewModel/Status = 'Open' or LoanWorkflowAlertViewModel/Status = 'Closed'">
                <td class="left">
                  <xsl:value-of select='LoanWorkflowAlertViewModel/WireId' />
                </td>
              </xsl:if>
              <td class="left">
                <xsl:value-of select="LoanWorkflowAlertViewModel/InitiatedBy"/>
              </td>
              <xsl:if test="count(LoanWorkflowAlertViewModel/ApprovalStatuses/ApprovalStatus) != 0">
                <td class="left" >
                  <xsl:for-each select="LoanWorkflowAlertViewModel/ApprovalStatuses/ApprovalStatus">
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
              </xsl:if>
            </tr>
          </tbody>
        </table>
      </p>
    </html>
  </xsl:template>
</xsl:stylesheet>
