import { useState, useEffect } from 'react';
import FoodEntryList from '../../components/food-entry-list';
import DateFilter from '../../components/date-filter';
import AddFoodEntry from '../../components/add-food-entry';
import { foodEntryService } from '../../services/food-entry-service';
import { alertService } from '../../services/alert-service';

export default function EntriesPage() {
    const [foodEntriesResult, setFoodEntriesResult] = useState({});
    const [loading, setLoading] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [filter, setFilter] = useState({ page: 1, pageSize: 10, from: '', to: '' });
    const [selectedFoodEntry, setSelectedFoodEntry] = useState({});

    useEffect(() => {
        list(filter.page, filter.pageSize, filter.from, filter.to);
        return () => {
            setLoading(false);
            setFoodEntriesResult({});
        }
    }, [filter]);

    const list = async (page, pageSize, from, to) => {
        setLoading(true);
        const data = await foodEntryService.getFoodEntries(page, pageSize, from, to)
            .catch(e => {
                alertService.error(e.toString());
                return {};
            });
        setFoodEntriesResult(data);
        setLoading(false);
    }

    const listFoodEntries = (dates) => {
        setFilter({ ...filter, from: dates.from, to: dates.to });
    }

    const onPageChange = (page) => {
        setFilter({ ...filter, page: page });
    }

    const openModal = (foodEntry) => {
        if (foodEntry) {
            setSelectedFoodEntry(foodEntry);
        }
        setIsModalOpen(true);
    }

    const closeModal = () => {
        setIsModalOpen(false);
    }

    return (
        <div className="container px-6 mx-auto grid">
            <div
                className="flex my-6  items-center justify-between p-4 mb-8 text-sm font-semibold text-purple-100 bg-purple-600 rounded-lg shadow-md focus:outline-none focus:shadow-outline-purple">
                <div className="flex items-center">
                    <span className="text-xl"> Food Entries</span>
                </div>
                <div className="flex items-center">
                    <button className="font-bold py-2 px-4 rounded inline-flex items-center" onClick={openModal}>
                        <span className="material-icons material-icons-outlined">add</span> <span className="text-lg"> Add</span>
                    </button>
                </div>
            </div>

            <AddFoodEntry isOpen={isModalOpen} close={closeModal} isAdmin={true} selectedFoodEntry={selectedFoodEntry} />

            <DateFilter onList={listFoodEntries} loading={loading} />

            <FoodEntryList foodEntries={foodEntriesResult.foodEntries} pageSize={filter.pageSize} page={filter.page}
                totalCount={foodEntriesResult.totalCount} onPageChange={onPageChange} isAdmin={true} showModal={openModal} />

            {!foodEntriesResult || !foodEntriesResult.foodEntries || foodEntriesResult.foodEntries.length === 0 && <div className="px-4 py-3 mb-8 bg-white rounded-lg shadow-md">
                <span className="text-gray-700">No food entry found. Please add your first food entry.</span>
            </div>}

        </div>
    )
}