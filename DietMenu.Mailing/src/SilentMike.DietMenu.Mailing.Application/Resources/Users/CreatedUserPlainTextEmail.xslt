<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="text" indent="yes" omit-xml-declaration="yes" />
	
  <xsl:template match="SendCreatedUserEmail">
	<xsl:text>&#10;</xsl:text>
	<xsl:text>Hello,</xsl:text>
	<xsl:text>&#10;</xsl:text>
	<xsl:text>DietMenu </xsl:text><xsl:value-of select="UserName"/><xsl:text> account was created in a family </xsl:text><xsl:value-of select="FamilyName"/><xsl:text>.</xsl:text>
	<xsl:text>&#10;</xsl:text>
	<xsl:text>LOGIN: </xsl:text><xsl:value-of select="LoginUrl"/>
	<xsl:text>&#10;</xsl:text>
	<xsl:text>-- The DietMenu team</xsl:text>
  </xsl:template>
</xsl:stylesheet>