<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="text" indent="yes" omit-xml-declaration="yes" />
	
  <xsl:template match="SendImportedCoreDataEmail">
	<xsl:text>&#10;</xsl:text>
	<xsl:text>Hello,</xsl:text>
	<xsl:text>&#10;</xsl:text>
	  <xsl:text>Core data have been imported.</xsl:text>
	  <xsl:text>&#10;</xsl:text>
	  <xsl:text>Server: </xsl:text><xsl:value-of select="Server"/>
	  <xsl:text>&#10;</xsl:text>
	  <xsl:text>Status:</xsl:text>
	  <xsl:choose>
		  <xsl:when test="IsSuccess='false'">
			  <xsl:text>FAILED</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
			  <xsl:text>SUCCESS</xsl:text>
		  </xsl:otherwise>
	  </xsl:choose>
	  <xsl:text>&#10;</xsl:text>
	  <xsl:if test="IsSuccess='false'">
		  <xsl:text>Errors:</xsl:text>
		  <xsl:for-each select="DataErrors">
			  <xsl:apply-templates select="." />
		  </xsl:for-each>
	  </xsl:if>
	  <xsl:text>&#10;</xsl:text>
	<xsl:text>-- The DietMenu team</xsl:text>
  </xsl:template>

	<xsl:template match="ImportedCoreDataAreaErrors">
		<xsl:text>&#10;</xsl:text>
		<xsl:text>- </xsl:text><xsl:value-of select="DataArea"/>
		<xsl:for-each select="Errors">
			<xsl:apply-templates select="." />
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="ImportedCoreDataError">
		<xsl:text>&#10;</xsl:text>
		<xsl:text>    - </xsl:text><xsl:value-of select="Code"/>
		<xsl:for-each select="Messages/string">
			<xsl:text>&#10;</xsl:text>
			<xsl:text>        - </xsl:text><xsl:value-of select="."/>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>