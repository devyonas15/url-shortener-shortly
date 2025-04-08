import { z, ZodType } from 'zod';
import { GenerateUrlRequest } from '../../common/types/UrlDTO';

// URL Generation Form Schema, bandaid solution from https://github.com/colinhacks/zod/issues/2236 to bypass url() validation bug.
const UrlGenerationFormSchema: ZodType<GenerateUrlRequest> = z.object({
  longUrl: z.string().refine(value => {
    const urlPattern = new RegExp(
      '^(https?:\\/\\/|www\\.)' +
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' +
        '((\\d{1,3}\\.){3}\\d{1,3}))' +
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' +
        '(\\?[;&a-z\\d%_.~+=-]*)?' +
        '(\\#[-a-z\\d_]*)?$',
      'i'
    );
    return urlPattern.test(value);
  }),
});

export { UrlGenerationFormSchema };
