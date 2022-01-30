<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="text" indent="yes" omit-xml-declaration="yes" />
	
  <xsl:template match="SendVerifyUserEmail">
	<xsl:text>&#10;</xsl:text>
	<xsl:text>Hello,</xsl:text>
	<xsl:text>&#10;</xsl:text>
	<xsl:text>DietMenu </xsl:text><xsl:value-of select="UserName"/><xsl:text> account was created.</xsl:text>
	<xsl:text>&#10;</xsl:text>
	<xsl:text>ACTIVATE ACCOUNT: </xsl:text><xsl:value-of select="Url"/>
	<xsl:text>&#10;</xsl:text>
	<xsl:text>-- The DietMenu team</xsl:text>
  </xsl:template>
</xsl:stylesheet>