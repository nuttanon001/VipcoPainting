export interface Color {
    ColorItemId: number;
    ColorCode?: string;
    ColorName?: string;
    VolumeSolids?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    // view model
    VolumeSolidsString?: string;
}