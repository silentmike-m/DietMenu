<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" doctype-public="-//W3C//DTD XHTML 1.1//EN" doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"/>
    
  <xsl:template match="SendImportedCoreDataEmail">
    <html>
      <head>
        <style>
          html,body { 
            font-family: "Roboto",sans-serif;
            font-size: .875rem;
            font-weight: 400;
            line-height: 1.375rem;
            letter-spacing: .0071428571em;
            line-height: 1.5;
            margin: 0;
            padding: 0;
            text-rendering: optimizeLegibility;
            word-break: normal;
          }

          body {
            background-color: #a5e887;
            color: rgba(0,0,0,.87);
          }

          .container {
            height: 100%;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
          background-color: #a5e887;
          }

          .content {
            border-radius: 4px;
            background-color: #fff;
            box-shadow: 0 3px 5px -1px rgba(0,0,0,.2),0 5px 8px 0 rgba(0,0,0,.14),0 1px 14px 0 rgba(0,0,0,.12) !important;
            border-color: #fff;
            border-width: thin;
            display: block;
            max-width: 100%;
            outline: none;
            text-decoration: none;
            transition-property: box-shadow,opacity;
            word-wrap: break-word;
            position: relative;
            white-space: normal;
            width: 30%;
            min-width: 350px;
          }

          .content-header {
            align-items: center;
            display: flex;
            flex-wrap: wrap;
            font-size: 1.25rem;
            font-weight: 300
            letter-spacing: .0125em;
            line-height: 2rem;
            word-break: break-all;
            padding: 16px;
          }

          .logo {
            margin-right: 10px;
          }

          .content-text {
            padding: 16px;
          }

          .content-button {
            height: 28px;
            min-width: 50px;
            padding: 0 12.4444444444px;
            color: #fff;
            background-color: #00aa95;
            border-color: #00aa95;
            font-size: 0.75rem;
            align-items: center;
            border-radius: 4px;
            display: inline-flex;
            flex: 0 0 auto;
            font-weight: 500;
            letter-spacing: 0.0892857143em;
            justify-content: center;
            text-decoration: none;
          }
        </style>
      </head>
      <body>
        <div class="container">
          <div class="content">
            <div class="content-header">
              <img class="logo" src="cid:logoId" alt="Dietkowanie" />
              <span class="title font-weight-light">DietMenu - core data imported</span>
            </div>
            <div class="content-text">
                <p>Hello,</p>
                <p>Core data have been imported.</p>
                <p>Server: <strong><xsl:value-of select="Server"/></strong></p>
                <p>Status: 
                    <xsl:choose>
                        <xsl:when test="IsSuccess='false'">
                            <span style="color:red;"><xsl:text>FAILED</xsl:text></span>
                        </xsl:when>
                        <xsl:otherwise>
                            <span style="color:green;"><xsl:text>SUCCESS</xsl:text></span>
                        </xsl:otherwise>
                    </xsl:choose>
                </p>
                <xsl:if test="IsSuccess='false'">
                    <p>Errors:</p>
                    <ul>
                        <xsl:for-each select="DataErrors">
                            <xsl:apply-templates select="." />
                        </xsl:for-each>
                    </ul>
                </xsl:if>
              <p>-- The DietMenu team</p>
            </div>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="ImportedCoreDataAreaErrors">
      <li><xsl:value-of select="DataArea"/>
          <ul>
              <xsl:for-each select="Errors">
                  <xsl:apply-templates select="." />
              </xsl:for-each>
          </ul>
      </li>
  </xsl:template>

  <xsl:template match="ImportedCoreDataError">
      <li><xsl:value-of select="Code"/>
          <ul>
              <xsl:for-each select="Messages/string">
                  <li><xsl:value-of select="." /></li>
              </xsl:for-each>
          </ul>
      </li>
  </xsl:template>
</xsl:stylesheet>