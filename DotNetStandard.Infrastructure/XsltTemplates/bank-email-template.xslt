<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <body>
        <div style="font-family: Arial;">
          <strong>
            Hi <xsl:value-of select="EmailTemplateViewModel/BankCode"/>,
          </strong>
          <br />
          <br />
          <span>Kindly collapse the box position(s) below. Thank you!</span>
          <br />
          <br />
          <table style="border-width: 1px; border-color: gray; background-color: #e4e3e3; border-collapse: collapse; font-family: Arial; font-size:14px" cellpadding="4">
            <thead>
              <tr style="background-color: #00a6d3;color:white">
                <td align="center" style="border: 1px solid white">
                  <strong>Fund Name</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>Account No</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>Product Description</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>Isin</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>Ticker</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>Cusip</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>Sedol</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>QTY</strong>
                </td>
                <td align="center" style="border: 1px solid white">
                  <strong>CPTY</strong>
                </td>
              </tr>
            </thead>
            <tbody>
              <xsl:for-each select="EmailTemplateViewModel/BoxedPositions/PositionDTO">
                <tr>
                  <td align="center" width="100" style="border: 1px solid white">
                    <xsl:value-of select="LocationAccount"/>
                  </td>
                  <td align="center" width="150" style="border: 1px solid white">
                    <xsl:value-of select="AccountNumber"/>
                  </td>
                  <td width="300" style="border: 1px solid white">
                    <xsl:value-of select="ProductDescription"/>
                  </td>
                  <td align="center" width="150" style="border: 1px solid white">
                    <xsl:value-of select="Isin"/>
                  </td>
                  <td align="center" width="100" style="border: 1px solid white">
                    <xsl:value-of select="Ticker"/>
                  </td>
                  <td align="center" width="100" style="border: 1px solid white">
                    <xsl:value-of select="Cusip"/>
                  </td>
                  <td align="center" width="100" style="border: 1px solid white">
                    <xsl:value-of select="Sedol"/>
                  </td>
                  <td align="right" width="100" style="border: 1px solid white">
                    <xsl:value-of select="Qty"/>
                  </td>
                  <td align="center" width="100" style="border: 1px solid white">
                    <xsl:value-of select="Counterparty"/>
                  </td>
                </tr>
              </xsl:for-each>
            </tbody>
          </table>
          <br />
          Best Regards,
          <br />
          AQR Team
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
