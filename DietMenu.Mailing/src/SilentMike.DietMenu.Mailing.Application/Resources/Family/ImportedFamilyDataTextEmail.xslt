<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns="http://www.w3.org/1999/xhtml">
    <xsl:output method="text" indent="yes" omit-xml-declaration="yes" />

    <xsl:template match="SendImportedFamilyDataEmail">
        <xsl:text>&#10;</xsl:text>
        <xsl:text>Hello,</xsl:text>
        <xsl:text>&#10;</xsl:text>
        <xsl:text>Family data have been imported.</xsl:text>
        <xsl:text>&#10;</xsl:text>
        <xsl:text>Server: </xsl:text><xsl:value-of select="Server" />
        <xsl:text>&#10;</xsl:text>
        <xsl:text>FamilyId: </xsl:text><xsl:value-of select="FamilyId" />
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
            <xsl:if test="ErrorCode">
                <xsl:text>&#10;</xsl:text>
                <xsl:text>- </xsl:text><xsl:value-of select="Code" /><xsl:text>: </xsl:text><xsl:value-of
                    select="Message" />
            </xsl:if>
            <xsl:for-each select="Results">
                <xsl:apply-templates select="." />
            </xsl:for-each>
            <xsl:text>&#10;</xsl:text>
            <xsl:text>Please contact DietMenu support.</xsl:text>
        </xsl:if>
        <xsl:text>&#10;</xsl:text>
        <xsl:text>-- The DietMenu team</xsl:text>
    </xsl:template>

    <xsl:template match="ImportedFamilyDataResult">
        <xsl:text>&#10;</xsl:text>
        <xsl:text>- </xsl:text><xsl:value-of select="DataArea" />
        <xsl:for-each select="Errors">
            <xsl:apply-templates select="." />
        </xsl:for-each>
    </xsl:template>

    <xsl:template match="ImportedFamilyDataError">
        <xsl:text>&#10;</xsl:text>
        <xsl:text>    - </xsl:text><xsl:value-of select="Code" /><xsl:text>: </xsl:text><xsl:value-of
            select="Message" />
    </xsl:template>
</xsl:stylesheet>
