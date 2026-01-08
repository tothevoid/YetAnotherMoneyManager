import { useCallback, useEffect, useState } from "react";
import { createBrokerAccountTaxDeduction, deleteBrokerAccountTaxDeduction, getBrokerAccountTaxDeductions, updateBrokerAccountTaxDeduction } from "../../../api/brokers/BrokerAccountTaxDeductionApi";
import { BrokerAccountTaxDeductionEntity } from "../../../models/brokers/BrokerAccountTaxDeductionEntity";
import { Nullable } from "../../../shared/utilities/nullable";

export interface TaxDecutionsQuery {
	brokerAccountId: Nullable<string>
}

export const useBrokerAccountTaxDeductions = (queryParameters: TaxDecutionsQuery, 
    onDataChanged: () => void) => {

    const [taxDeductions, setTaxDeductions] = useState<BrokerAccountTaxDeductionEntity[]>([]);
    const [isTaxDeductionsLoading, setLoading] = useState(false);

    const [error, setError] = useState<string | null>(null);
    const [taxDeductionsQueryParameters, setTaxDeductionsQueryParameters] = useState<TaxDecutionsQuery>(queryParameters);

    const fetchData = useCallback(async () => {
        setLoading(true)
        try {
            const deductions = await getBrokerAccountTaxDeductions(taxDeductionsQueryParameters);
            setTaxDeductions(deductions);
        } catch (err: any) {
            debugger;
            setError(err.message || 'Error loading data');
        } finally {
            setLoading(false);
        }
    }, [])

    useEffect(() => {
        fetchData();
    }, [fetchData])

    const createTaxDeductionEntity = async (createdTaxDeduction: BrokerAccountTaxDeductionEntity) => {
        await createBrokerAccountTaxDeduction(createdTaxDeduction);

        await fetchData();
        onDataChanged();
    }

    const updateTaxDeductionEntity = async (updatedTaxDeduction: BrokerAccountTaxDeductionEntity) => {
        await updateBrokerAccountTaxDeduction(updatedTaxDeduction);
        
        await fetchData();
        onDataChanged();
    }

    const deleteTaxDeductionEntity = async (deletedTaxDeduction: BrokerAccountTaxDeductionEntity) => {
        const taxDeductionDeleted = await deleteBrokerAccountTaxDeduction(deletedTaxDeduction.id);
        if (!taxDeductionDeleted) {
            return;
        }

        await fetchData();
        onDataChanged();
    }

    return {
        taxDeductions,
        isTaxDeductionsLoading,
        error,
        createTaxDeductionEntity,
        updateTaxDeductionEntity,
        deleteTaxDeductionEntity,
        reloadTaxDeductions: fetchData,
        taxDeductionsQueryParameters,
        setTaxDeductionsQueryParameters
    }
}
